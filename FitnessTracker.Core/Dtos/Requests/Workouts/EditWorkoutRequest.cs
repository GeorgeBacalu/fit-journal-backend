namespace FitnessTracker.Core.Dtos.Requests.Workouts;

public record EditWorkoutRequest : AddWorkoutRequest
{
    public Guid Id { get; init; }
}
