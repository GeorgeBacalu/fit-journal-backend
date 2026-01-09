namespace FitJournal.Core.Dtos.Responses.WorkoutExercises;

public record WorkoutExerciseResponse : ShortWorkoutExerciseResponse
{
    public Guid WorkoutId { get; init; }
}
