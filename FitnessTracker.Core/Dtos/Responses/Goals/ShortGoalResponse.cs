using FitnessTracker.Domain.Enums;

namespace FitnessTracker.Core.Dtos.Responses.Goals;

public record ShortGoalResponse
{
    public required string Name { get; init; }
    public GoalType Type { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
}
