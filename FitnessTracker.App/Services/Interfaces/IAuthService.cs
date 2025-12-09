using FitnessTracker.App.Dtos.Requests.Auth;

namespace FitnessTracker.App.Services.Interfaces;
public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request, CancellationToken token = default);
}
