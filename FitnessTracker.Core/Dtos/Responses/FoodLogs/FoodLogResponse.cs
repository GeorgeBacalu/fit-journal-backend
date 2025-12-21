namespace FitnessTracker.Core.Dtos.Responses.FoodLogs;

public record FoodLogResponse
{
    public DateTime Date { get; set; }
    public int Servings { get; set; }
    public int Quantity { get; set; }
}
