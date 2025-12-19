namespace FitnessTracker.Core.Dtos.Responses.Goals;

public record GoalsResponse
{
    public IEnumerable<ShortGoalResponse> Goals { get; init; } = [];
    public int TotalCount { get; init; }
}
