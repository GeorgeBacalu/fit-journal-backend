using AutoMapper;
using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.Users;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Interfaces.Services.Admin;
using FitnessTracker.Core.Interfaces.Validators;

namespace FitnessTracker.Core.Services.Admin;

public class AdminUserService(IUnitOfWork unitOfWork, IMapper mapper, IUserValidator userValidator)
    : BusinessService(unitOfWork, mapper), IAdminUserService
{
    private readonly IUserValidator _userValidator = userValidator;

    public async Task EditAsync(EditUserRequest request, Guid id, CancellationToken token)
    {
        await _userValidator.ValidateEditAsync(request, id, token);

        var user = await _unitOfWork.Users.GetByIdTrackedAsync(id, token)
            ?? throw new NotFoundException(BusinessErrors.Users.IdNotFound(id));

        _mapper.Map(request, user);
        await _unitOfWork.CommitAsync(token);
    }
}
