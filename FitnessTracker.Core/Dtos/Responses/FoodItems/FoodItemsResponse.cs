namespace FitnessTracker.Core.Dtos.Responses.FoodItems;

public record FoodItemsResponse
{
    public IEnumerable<ShortFoodItemResponse> FoodItems { get; init; } = [];
    public int TotalCount { get; init; }
}
