using FitJournal.Core.Dtos.Responses.Auth;
using FitJournal.Core.Interfaces.Services;
using FitJournal.Domain.Entities;
using FitJournal.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace FitJournal.Infra.Services;

public sealed class ExternalAuthCodeStore(AppDbContext db) : IExternalAuthCodeStore
{
    public async Task<string> StoreAsync(LoginResponse tokens, TimeSpan lifetime, CancellationToken token)
    {
        var now = DateTime.UtcNow;
        await db.OAuthExchangeCodes
            .Where(code => code.ExpiresAt <= now || code.ConsumedAt != null)
            .ExecuteDeleteAsync(token);

        var code = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        db.OAuthExchangeCodes.Add(new()
        {
            CodeHash = Hash(code),
            TokenPayload = JsonSerializer.Serialize(tokens),
            ExpiresAt = now.Add(lifetime)
        });
        await db.SaveChangesAsync(token);
        return code;
    }

    public async Task<LoginResponse?> RedeemAsync(string code, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length != 64)
            return null;

        var now = DateTime.UtcNow;
        var codeHash = Hash(code);
        var exchange = await db.OAuthExchangeCodes
            .AsNoTracking()
            .Where(candidate => candidate.CodeHash == codeHash && candidate.ConsumedAt == null && candidate.ExpiresAt > now)
            .Select(candidate => new { candidate.Id, candidate.TokenPayload })
            .SingleOrDefaultAsync(token);

        if (exchange == null)
            return null;

        var consumed = await db.OAuthExchangeCodes
            .Where(candidate => candidate.Id == exchange.Id && candidate.ConsumedAt == null && candidate.ExpiresAt > now)
            .ExecuteUpdateAsync(update => update.SetProperty(candidate => candidate.ConsumedAt, now), token);

        return consumed == 1
            ? JsonSerializer.Deserialize<LoginResponse>(exchange.TokenPayload)
            : null;
    }

    private static string Hash(string code) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(code)));
}
