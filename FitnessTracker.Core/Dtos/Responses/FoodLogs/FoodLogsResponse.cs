using FitnessTracker.Core.Interfaces.Services;

namespace FitnessTracker.Core.Dtos.Responses.FoodLogs;

public record FoodLogsResponse : IFoodLogsResponse
{
    public IEnumerable<ShortFoodLogResponse> FoodLogs { get; init; } = [];
    public int TotalCount { get; init; }
}
