using FitJournal.Core.Interfaces.Services;

namespace FitJournal.Core.Dtos.Responses.FoodLogs;

public record FoodLogsResponse : IFoodLogsResponse
{
    public IEnumerable<ShortFoodLogResponse> FoodLogs { get; init; } = [];
    public int TotalCount { get; init; }
}
