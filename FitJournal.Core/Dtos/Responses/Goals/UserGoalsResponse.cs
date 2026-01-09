namespace FitJournal.Core.Dtos.Responses.Goals;

public record UserGoalsResponse
{
    public Guid UserId { get; init; }
    public string? UserName { get; init; }
    public IEnumerable<ShortGoalResponse> Goals { get; init; } = [];
}
