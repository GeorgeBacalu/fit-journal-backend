using FitJournal.Core.Dtos.Requests.Auth;
using FitJournal.Core.Dtos.Responses.Auth;

namespace FitJournal.Core.Interfaces.Services;

public interface IAuthService : IBusinessService
{
    Task RegisterAsync(RegisterRequest request, CancellationToken token);

    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken token);

    Task<RefreshResponse> RefreshAsync(RefreshRequest request, CancellationToken token);

    Task DeleteAsync(Guid id, CancellationToken token);
}
