namespace FitnessTracker.Core.Dtos.Responses.FoodItems;

public record FoodItemsResponse
{
    public IEnumerable<FoodItemResponse> FoodItems { get; init; } = [];
    public int TotalCount { get; init; }
}
