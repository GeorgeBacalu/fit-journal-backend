using FitnessTracker.Infra.Config;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace FitnessTracker.Test.Integration;

public static class AuthConfig
{
    public static void EnsureInitialized()
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        AppConfig.Init(new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Auth:Issuer"] = "https://localhost:5000/",
            ["Auth:Audience"] = "http://localhost:4200/",
            ["Auth:Secret"] = "00000000000000000000000000000000",
            ["Auth:AccessTokenLifetimeMinutes"] = "15",
            ["Auth:RefreshTokenLifetimeDays"] = "30"
        }).Build());
    }
}
