namespace FitnessTracker.Core.Dtos.Requests.FoodItems;

public record EditFoodItemRequest : AddFoodItemRequest
{
    public Guid Id { get; init; }
}
