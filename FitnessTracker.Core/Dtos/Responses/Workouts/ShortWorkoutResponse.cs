namespace FitnessTracker.Core.Dtos.Responses.Workouts;

public class ShortWorkoutResponse
{
    public required string Name { get; set; }
    public int DurationMinutes { get; set; }
    public DateTime StartedAt { get; set; }
}
