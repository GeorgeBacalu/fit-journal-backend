namespace FitnessTracker.App.Dtos.Requests.Workouts;

public class DeleteWorkoutsRequest
{
    public IEnumerable<Guid> Ids { get; init; } = [];
}
