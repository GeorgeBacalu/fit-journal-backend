using FitnessTracker.Core.Dtos.Requests.Auth;
using FitnessTracker.Core.Dtos.Responses.Auth;

namespace FitnessTracker.Core.Interfaces.Services;

public interface IAuthService : IBusinessService
{
    Task RegisterAsync(RegisterRequest request, CancellationToken token);

    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken token);

    Task DeleteAsync(Guid id, CancellationToken token);
}
