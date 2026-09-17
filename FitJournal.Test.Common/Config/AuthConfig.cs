using FitJournal.Core.Config;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace FitJournal.Test.Common.Config;

public static class AuthConfig
{
    public static void EnsureInitialized()
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        AppConfig.Init(new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:FitJournalDb"] = "Data Source=:memory:",
                ["Auth:Issuer"] = "test_issuer",
                ["Auth:Audience"] = "test_audience",
                ["Auth:Secret"] = "test_secret_key_that_is_long_enough_for_hmac_sha256_signing",
                ["Auth:AccessTokenLifetimeMinutes"] = "15",
                ["Auth:RefreshTokenLifetimeDays"] = "30",
                ["Email:User"] = "test@fitjournal.local",
                ["Email:Password"] = "test-password",
                ["Email:MailBoxName"] = "FitJournal Tests",
                ["Email:SmtpHost"] = "localhost",
                ["Email:SmtpPort"] = "2525",
                ["ExternalAuth:FrontendUrl"] = "https://app.fitjournal.test"
            }).Build());
    }
}
