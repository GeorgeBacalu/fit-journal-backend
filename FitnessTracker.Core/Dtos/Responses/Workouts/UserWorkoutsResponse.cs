namespace FitnessTracker.Core.Dtos.Responses.Workouts;

public record UserWorkoutsResponse
{
    public Guid UserId { get; init; }
    public string? UserName { get; init; }
    public IEnumerable<ShortWorkoutResponse> Workouts { get; init; } = [];
}
