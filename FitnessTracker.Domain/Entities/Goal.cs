using FitnessTracker.Domain.Enums;

namespace FitnessTracker.Domain.Entities;

public class Goal : BaseEntity, IUserOwnedEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public GoalType Type { get; set; }
    public int TargetWeight { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public bool IsAchieved { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }
}
