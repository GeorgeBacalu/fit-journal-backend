using FitJournal.Core.Dtos.Requests.Auth;
using FitJournal.Core.Dtos.Requests.Users;
using FitJournal.Core.Interfaces.Services;

namespace FitJournal.Core.Interfaces.Validators;

public interface IUserValidator : IBusinessService
{
    Task ValidateRegisterAsync(RegisterRequest request, CancellationToken token);

    Task ValidateEditAsync(EditUserRequest request, Guid id, CancellationToken token);
}
