namespace FitJournal.Core.Dtos.Responses.Workouts;

public record ShortWorkoutResponse
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public int DurationMinutes { get; init; }
    public DateTime StartedAt { get; init; }
}
