namespace FitnessTracker.Domain.Entities;

public interface IUserOwnedEntity
{
    Guid UserId { get; }
}
