namespace FitnessTracker.Core.Dtos.Responses.FoodItems;

public record ShortFoodItemResponse
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public decimal Calories { get; init; }
    public decimal Protein { get; init; }
    public decimal Carbs { get; init; }
    public decimal Fat { get; init; }
}
