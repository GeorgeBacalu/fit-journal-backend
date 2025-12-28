using Microsoft.Extensions.Configuration;

namespace FitnessTracker.Core.Config;

public static class AppConfig
{
    public static ConnectionStrings ConnectionStrings { get; private set; } = null!;
    public static Auth Auth { get; private set; } = null!;

    public static void Init(IConfiguration config)
    {
        ConnectionStrings = config.GetRequiredSection(nameof(ConnectionStrings)).Get<ConnectionStrings>()!;
        Auth = config.GetRequiredSection(nameof(Auth)).Get<Auth>()!;
    }
}

public record ConnectionStrings(string FitnessTrackerDb);
public record Auth(string Issuer, string Audience, string Secret, int AccessTokenLifetimeMinutes, int RefreshTokenLifetimeDays);
