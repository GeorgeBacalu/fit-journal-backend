namespace FitJournal.Domain.Entities;

public class OAuthExchangeCode : BaseEntity
{
    public required string CodeHash { get; init; }
    public required string TokenPayload { get; init; }
    public DateTime ExpiresAt { get; init; }
    public DateTime? ConsumedAt { get; set; }
}
