using AutoMapper;
using AutoMapper.QueryableExtensions;
using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.Users;
using FitnessTracker.Core.Dtos.Responses.Users;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Extensions.Pagination;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Interfaces.Services;
using FitnessTracker.Core.Interfaces.Validators;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Core.Services;

public class UserService(IUnitOfWork unitOfWork, IMapper mapper, IUserValidator userValidator)
    : BusinessService(unitOfWork, mapper), IUserService
{
    private readonly IUserValidator _userValidator = userValidator;

    public async Task<UsersResponse> GetAllAsync(UserPaginationRequest request, CancellationToken token)
    {
        var baseQuery = _unitOfWork.Users.GetAllQuery().AddFilters(request);

        var users = await baseQuery
            .AddSorting(request)
            .AddPaging(request)
            .ProjectTo<ShortUserResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(token);
        var totalCount = await baseQuery.CountAsync(token);

        return new() { Users = users, TotalCount = totalCount };
    }

    public async Task<UserResponse> GetByIdAsync(Guid id, CancellationToken token)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id, token)
            ?? throw new NotFoundException(BusinessErrors.Users.IdNotFound(id));

        return _mapper.Map<UserResponse>(user);
    }

    public async Task EditAsync(EditUserRequest request, Guid id, CancellationToken token)
    {
        await _userValidator.ValidateEditAsync(request, id, token);

        var user = await _unitOfWork.Users.GetByIdTrackedAsync(id, token)
            ?? throw new NotFoundException(BusinessErrors.Users.IdNotFound(id));

        _mapper.Map(request, user);
        await _unitOfWork.CommitAsync(token);
    }
}
