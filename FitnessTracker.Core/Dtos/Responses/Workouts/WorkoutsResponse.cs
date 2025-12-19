namespace FitnessTracker.Core.Dtos.Responses.Workouts;

public class WorkoutsResponse
{
    public IEnumerable<ShortWorkoutResponse> Workouts { get; init; } = [];
    public int TotalCount { get; set; }
}
