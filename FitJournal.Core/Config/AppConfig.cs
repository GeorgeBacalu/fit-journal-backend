using Microsoft.Extensions.Configuration;

namespace FitJournal.Core.Config;

public static class AppConfig
{
    public static ConnectionStrings ConnectionStrings { get; private set; } = null!;
    public static Auth Auth { get; private set; } = null!;
    public static Email Email { get; private set; } = null!;

    public static void Init(IConfiguration config)
    {
        ConnectionStrings = config.GetRequiredSection(nameof(ConnectionStrings)).Get<ConnectionStrings>()!;
        Auth = config.GetRequiredSection(nameof(Auth)).Get<Auth>()!;
        Email = config.GetRequiredSection(nameof(Email)).Get<Email>()!;
    }
}

public record ConnectionStrings(string FitJournalDb);
public record Auth(string Issuer, string Audience, string Secret, int AccessTokenLifetimeMinutes, int RefreshTokenLifetimeDays);
public record Email(string User, string Password, string MailBoxName, string SmtpHost, int SmtpPort);
