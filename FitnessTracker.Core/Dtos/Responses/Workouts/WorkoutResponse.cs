namespace FitnessTracker.Core.Dtos.Responses.Workouts;

public record WorkoutResponse
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
    public int DurationMinutes { get; init; }
    public DateTime StartedAt { get; init; }
}
