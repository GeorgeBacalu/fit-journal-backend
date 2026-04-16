namespace FitJournal.Core.Dtos.Common.WorkoutExercises;

public record WorkoutExerciseDto
{
    public Guid Id { get; init; }
    public Guid ExerciseId { get; init; }

    public int Sets { get; init; }
    public int Reps { get; init; }
    public decimal WeightUsed { get; init; }
}
