namespace FitJournal.Core.Dtos.Responses.FoodLogs;

public record UsersFoodLogsResponse : IFoodLogsResponse
{
    public IEnumerable<UserFoodLogsResponse> Users { get; init; } = [];
    public int TotalCount { get; init; }
}
