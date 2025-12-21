namespace FitnessTracker.Core.Dtos.Responses.FoodLogs;

public record ShortFoodLogResponse
{
    public DateTime Date { get; init; }
    public int Servings { get; init; }
    public int Quantity { get; init; }
}
