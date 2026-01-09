namespace FitnessTracker.Core.Dtos.Responses.ProgressLogs;

public record UserProgressLogsResponse
{
    public Guid UserId { get; init; }
    public string? UserName { get; init; }
    public IEnumerable<ShortProgressLogResponse> ProgressLogs { get; init; } = [];
}
