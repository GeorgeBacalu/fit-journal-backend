using FitJournal.Core.Dtos.Common.FoodItems;

namespace FitJournal.Core.Dtos.Responses.FoodLogs;

public record FoodLogResponse : ShortFoodLogResponse
{
    public FoodItemDto? FoodItem { get; init; }
}
