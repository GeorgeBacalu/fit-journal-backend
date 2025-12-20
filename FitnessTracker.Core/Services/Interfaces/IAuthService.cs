using FitnessTracker.Core.Dtos.Requests.Auth;
using FitnessTracker.Core.Dtos.Responses.Auth;

namespace FitnessTracker.Core.Services.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request, CancellationToken token);

    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken token);

    Task DeleteAsync(Guid id, CancellationToken token);
}
