using FitJournal.Core.Dtos.Responses.Auth;

namespace FitJournal.Core.Interfaces.Services;

public interface IExternalAuthCodeStore
{
    Task<string> StoreAsync(LoginResponse tokens, TimeSpan lifetime, CancellationToken token);
    Task<LoginResponse?> RedeemAsync(string code, CancellationToken token);
}
