namespace FitnessTracker.Core.Dtos.Requests.Workouts;

public record RemoveWorkoutsRequest
{
    public IEnumerable<Guid> Ids { get; init; } = [];
    public bool IsHardDelete { get; init; }
}
