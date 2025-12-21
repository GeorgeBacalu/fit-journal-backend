namespace FitnessTracker.Core.Dtos.Responses.FoodLogs;

public record FoodLogsResponse
{
    public IEnumerable<ShortFoodLogResponse> FoodLogs { get; init; } = [];
    public int TotalCount { get; init; }
}
