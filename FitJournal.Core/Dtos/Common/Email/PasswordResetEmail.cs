namespace FitJournal.Core.Dtos.Common.Email;

public record PasswordResetEmail
{
    public required string UserName { get; init; }
    public required string ResetLink { get; init; }
    public required DateTime ExpiresAt { get; init; }
}
