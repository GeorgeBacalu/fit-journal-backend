namespace FitnessTracker.Core.Dtos.Responses.Workouts;

public class GetWorkoutsResponse
{
    public IEnumerable<GetWorkoutResponse> Workouts { get; init; } = [];
}
