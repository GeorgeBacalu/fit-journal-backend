namespace FitJournal.Core.Dtos.Responses.Goals;

public record GoalResponse : ShortGoalResponse
{
    public string? Description { get; init; }
    public string? Notes { get; init; }
    public int TargetWeight { get; init; }
}
