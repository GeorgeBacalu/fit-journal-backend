using FitnessTracker.Core.Dtos.Requests.Auth;
using FitnessTracker.Core.Dtos.Responses.Auth;

namespace FitnessTracker.Core.Services.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request, CancellationToken token = default);
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken token = default);
    Task DeleteAccountAsync(Guid? id, Guid userId, CancellationToken token = default);
}
