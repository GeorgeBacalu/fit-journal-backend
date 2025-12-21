namespace FitnessTracker.Core.Dtos.Responses.FoodLogs;

public record FoodLogsResponse
{
    public IEnumerable<FoodLogResponse> FoodLogs { get; init; } = [];
    public int TotalCount { get; init; }
}
