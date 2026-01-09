namespace FitJournal.Core.Dtos.Responses.FoodLogs;

public record UserFoodLogsResponse
{
    public Guid UserId { get; init; }
    public string? UserName { get; init; }
    public IEnumerable<ShortFoodLogResponse> FoodLogs { get; init; } = [];
}
