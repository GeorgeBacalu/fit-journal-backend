namespace FitJournal.Domain.Entities;

public interface IUserOwnedEntity
{
    Guid UserId { get; }
}
