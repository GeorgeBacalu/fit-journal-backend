namespace FitJournal.Core.Dtos.Responses.FoodLogs;

public record ShortFoodLogResponse
{
    public Guid Id { get; init; }
    public DateTime Date { get; init; }
    public int Servings { get; init; }
    public decimal Quantity { get; init; }
}
