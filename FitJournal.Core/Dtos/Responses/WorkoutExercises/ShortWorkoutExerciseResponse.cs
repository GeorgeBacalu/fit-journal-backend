namespace FitJournal.Core.Dtos.Responses.WorkoutExercises;

public record ShortWorkoutExerciseResponse
{
    public Guid Id { get; init; }
    public int Sets { get; init; }
    public int Reps { get; init; }
    public decimal WeightUsed { get; init; }

    public Guid ExerciseId { get; init; }
}
