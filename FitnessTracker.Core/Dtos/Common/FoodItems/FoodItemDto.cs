using FitnessTracker.Domain.Enums.FoodItems;

namespace FitnessTracker.Core.Dtos.Common.FoodItems;

public record FoodItemDto
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public FoodCategory Category { get; init; }
    public FoodBrand Brand { get; init; }
}
