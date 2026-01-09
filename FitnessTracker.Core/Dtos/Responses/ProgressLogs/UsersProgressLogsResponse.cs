namespace FitnessTracker.Core.Dtos.Responses.ProgressLogs;

public record UsersProgressLogsResponse : IProgressLogsResponse
{
    public IEnumerable<UserProgressLogsResponse> Users { get; init; } = [];
    public int TotalCount { get; init; }
}
