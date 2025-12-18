namespace FitnessTracker.Core.Dtos.Requests.Workouts;

public record AddWorkoutRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
    public int DurationMinutes { get; init; }
    public DateTime StartedAt { get; init; }
}
