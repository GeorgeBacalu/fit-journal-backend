namespace FitnessTracker.App.Dtos.Responses.Workouts;

public class GetWorkoutsResponse
{
    public IEnumerable<GetWorkoutResponse> Workouts { get; init; } = [];
}
