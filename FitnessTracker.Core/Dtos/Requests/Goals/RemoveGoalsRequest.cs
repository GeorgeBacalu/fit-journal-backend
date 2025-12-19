namespace FitnessTracker.Core.Dtos.Requests.Goals;

public record RemoveGoalsRequest
{
    public IEnumerable<Guid> Ids { get; init; } = [];
    public bool IsHardDelete { get; set; }
}
