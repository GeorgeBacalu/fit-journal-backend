using FitnessTracker.Domain.Enums;

namespace FitnessTracker.Domain.Entities;

public class Goal : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public GoalType Type { get; set; }
    public int TargetWeight { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public bool IsAchieved { get; set; } = false;

    public Guid UserId { get; set; }
    public User? User { get; set; }
}
