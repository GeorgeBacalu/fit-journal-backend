using FitnessTracker.App.Dtos.Requests.Auth;
using FitnessTracker.App.Dtos.Responses;

namespace FitnessTracker.App.Services.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request, CancellationToken token = default);
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken token = default);
}
