using FitnessTracker.Domain.Enums.Goals;

namespace FitnessTracker.Core.Dtos.Responses.Goals;

public record ShortGoalResponse
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public GoalType Type { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public bool IsAchieved { get; init; }
}
