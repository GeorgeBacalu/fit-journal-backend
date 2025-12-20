using FitnessTracker.Infra.Config;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace FitnessTracker.Test.Integration;

public static class AuthConfig
{
    public static void EnsureInitialized()
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        AppConfig.Init(new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Auth:Issuer"] = "test_issuer",
                ["Auth:Audience"] = "test_audience",
                ["Auth:Secret"] = "test_secret_key",
                ["Auth:AccessTokenLifetimeMinutes"] = "15",
                ["Auth:RefreshTokenLifetimeDays"] = "30"
            })
            .Build());
    }
}
