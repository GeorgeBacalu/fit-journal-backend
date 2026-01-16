using AutoMapper;
using FitJournal.Core.Constants;
using FitJournal.Core.Dtos.Requests.Auth;
using FitJournal.Core.Dtos.Requests.Users;
using FitJournal.Core.Exceptions;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Core.Interfaces.Validators;
using FitJournal.Core.Services;

namespace FitJournal.Core.Validators;

public class UserValidator(IUnitOfWork unitOfWork, IMapper mapper)
    : BusinessService(unitOfWork, mapper), IUserValidator
{
    public async Task ValidateRegisterAsync(RegisterRequest request, CancellationToken token) =>
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
