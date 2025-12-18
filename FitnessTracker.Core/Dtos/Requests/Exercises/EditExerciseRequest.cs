namespace FitnessTracker.Core.Dtos.Requests.Exercises;

public record EditExerciseRequest : AddExerciseRequest
{
    public Guid Id { get; init; }
}
