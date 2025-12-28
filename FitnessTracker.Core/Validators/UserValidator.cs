using AutoMapper;
using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.Auth;
using FitnessTracker.Core.Dtos.Requests.Users;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Interfaces.Validators;
using FitnessTracker.Core.Services;

namespace FitnessTracker.Core.Validators;

public class UserValidator(IUnitOfWork unitOfWork, IMapper mapper)
    : BusinessService(unitOfWork, mapper), IUserValidator
{
    public async Task ValidateAddAsync(RegisterRequest request, CancellationToken token) =>
        await ValidateAsync(request, excludeId: null, token);

    public async Task ValidateEditAsync(EditUserRequest request, Guid id, CancellationToken token) =>
        await ValidateAsync(request, excludeId: id, token);

    private async Task ValidateAsync(IUserRequest request, Guid? excludeId, CancellationToken token)
    {
        if (await _unitOfWork.Users.AnyAsync(u => u.Name == request.Name && (excludeId == null || u.Id != excludeId), token))
            throw new BadRequestException(ValidationErrors.Common.NameTaken);

        if (await _unitOfWork.Users.AnyAsync(u => u.Email == request.Email && (excludeId == null || u.Id != excludeId), token))
            throw new BadRequestException(ValidationErrors.Users.EmailTaken);
    }
}
