using Microsoft.Extensions.Configuration;

namespace FitnessTracker.Infra.Config;
public static class AppConfig
{
    public static ConnectionStrings ConnectionStrings { get; private set; } = null!;

    public static void Init(IConfiguration config)
    {
        ConnectionStrings = config.GetSection(nameof(ConnectionStrings)).Get<ConnectionStrings>()!;
    }
}

public record ConnectionStrings(string FitnessTrackerDb);
