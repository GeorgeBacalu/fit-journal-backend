using FitnessTracker.Core.Dtos.Requests.Auth;
using FitnessTracker.Core.Dtos.Requests.Users;
using FitnessTracker.Core.Interfaces.Services;

namespace FitnessTracker.Core.Interfaces.Validators;

public interface IUserValidator : IBusinessService
{
    Task ValidateAddAsync(RegisterRequest request, CancellationToken token);

    Task ValidateEditAsync(EditUserRequest request, Guid id, CancellationToken token);
}
