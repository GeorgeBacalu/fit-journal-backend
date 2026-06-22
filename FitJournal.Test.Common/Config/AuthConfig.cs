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
                ["ConnectionStrings:FitJournalDb"] = "test",
                ["Auth:Issuer"] = "test_issuer",
                ["Auth:Audience"] = "test_audience",
                ["Auth:Secret"] = "test_secret_key",
                ["Auth:AccessTokenLifetimeMinutes"] = "15",
                ["Auth:RefreshTokenLifetimeDays"] = "30",
                ["Email:User"] = "test_email_username",
                ["Email:Password"] = "test_email_password",
                ["Email:MailBoxName"] = "test_email_mailbox",
                ["Email:SmtpHost"] = "test_email_smtp_host",
                ["Email:SmtpPort"] = "587"
            }).Build());
    }
}
