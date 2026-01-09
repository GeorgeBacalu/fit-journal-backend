namespace FitnessTracker.Core.Dtos.Responses.Workouts;

public record UsersWorkoutsResponse : IWorkoutsResponse
{
    public IEnumerable<UserWorkoutsResponse> Users { get; init; } = [];
    public int TotalCount { get; init; }
}
