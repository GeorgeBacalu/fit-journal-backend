namespace FitJournal.Core.Dtos.Responses.FoodItems;

public record FoodItemResponse : ShortFoodItemResponse
{
    public decimal Calories { get; init; }
    public decimal Protein { get; init; }
    public decimal Carbs { get; init; }
    public decimal Fat { get; init; }
}
