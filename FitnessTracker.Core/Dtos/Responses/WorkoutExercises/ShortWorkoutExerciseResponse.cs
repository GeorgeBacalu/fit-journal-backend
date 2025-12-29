namespace FitnessTracker.Core.Dtos.Responses.WorkoutExercises;

public record ShortWorkoutExerciseResponse
{
    public int Sets { get; init; }
    public int Reps { get; init; }
    public decimal WeightUsed { get; init; }

    public Guid ExerciseId { get; init; }
}
