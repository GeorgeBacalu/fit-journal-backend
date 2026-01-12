namespace FitJournal.Domain.Entities;

public class ResetToken : BaseEntity, IUserOwnedEntity
{
    public required string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool Used { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }
}
