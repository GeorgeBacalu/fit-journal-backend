namespace FitnessTracker.Core.Dtos.Responses.ProgressLogs;

public record ProgressLogsResponse
{
    public IEnumerable<ShortProgressLogResponse> ProgressLogs { get; init; } = [];
    public int TotalCount { get; init; }
}
