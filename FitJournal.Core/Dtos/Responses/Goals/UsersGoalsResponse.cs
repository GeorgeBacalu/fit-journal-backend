namespace FitJournal.Core.Dtos.Responses.Goals;

public record UsersGoalsResponse : IGoalsResponse
{
    public IEnumerable<UserGoalsResponse> Users { get; init; } = [];
    public int TotalCount { get; init; }
}
