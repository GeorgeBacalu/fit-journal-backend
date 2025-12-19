using FitnessTracker.Domain.Enums;

namespace FitnessTracker.Core.Dtos.Requests.Goals;

public record AddGoalRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public GoalType? Type { get; init; }
    public int TargetWeight { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
}
