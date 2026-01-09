using FitnessTracker.Domain.Enums.FoodItems;

namespace FitnessTracker.Core.Dtos.Responses.FoodItems;

public record ShortFoodItemResponse
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public FoodCategory Category { get; init; }
    public FoodBrand Brand { get; init; }
}
