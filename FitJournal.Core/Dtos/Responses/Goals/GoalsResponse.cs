namespace FitJournal.Core.Dtos.Responses.Goals;

public record GoalsResponse : IGoalsResponse
{
    public IEnumerable<ShortGoalResponse> Goals { get; init; } = [];
    public int TotalCount { get; init; }
}
