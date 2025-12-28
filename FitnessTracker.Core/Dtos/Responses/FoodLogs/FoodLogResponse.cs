using FitnessTracker.Core.Dtos.Common.FoodItems;

namespace FitnessTracker.Core.Dtos.Responses.FoodLogs;

public record FoodLogResponse : ShortFoodLogResponse
{
    public FoodItemDto? FoodItem { get; init; }
}
