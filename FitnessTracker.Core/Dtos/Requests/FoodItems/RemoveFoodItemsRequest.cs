namespace FitnessTracker.Core.Dtos.Requests.FoodItems;

public record RemoveFoodItemsRequest
{
    public IEnumerable<Guid> Ids { get; init; } = [];
    public bool IsHardDelete { get; init; }
}
