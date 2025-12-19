namespace FitnessTracker.Core.Dtos.Requests.FoodItems;

public class AddFoodItemRequest
{
    public required string Name { get; init; }
    public decimal? Calories { get; init; }
    public decimal? Protein { get; init; }
    public decimal? Carbs { get; init; }
    public decimal? Fat { get; init; }
}
