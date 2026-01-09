using AutoMapper;
using AutoMapper.QueryableExtensions;
using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.Users;
using FitnessTracker.Core.Dtos.Responses.Users;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Interfaces.Services;
using FitnessTracker.Core.Interfaces.Validators;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Core.Services;

public class UserService(IUnitOfWork unitOfWork, IMapper mapper, IUserValidator userValidator)
    : BusinessService(unitOfWork, mapper), IUserService
{
    private readonly IUserValidator _userValidator = userValidator;

    public async Task<UsersResponse> GetAllAsync(UserPaginationRequest request, CancellationToken token) => new()
    {
        TotalCount = await _unitOfWork.Users.GetAllBaseQuery(request).CountAsync(token),
        Users = await _unitOfWork.Users.GetAllQuery(request)
            .ProjectTo<ShortUserResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(token)
    };

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
