namespace FitnessTracker.Core.Dtos.Responses.Workouts;

public record WorkoutsResponse
{
    public IEnumerable<ShortWorkoutResponse> Workouts { get; init; } = [];
    public int TotalCount { get; init; }
}
