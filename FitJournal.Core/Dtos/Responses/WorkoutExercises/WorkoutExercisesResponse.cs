namespace FitJournal.Core.Dtos.Responses.WorkoutExercises;

public record WorkoutExercisesResponse : IWorkoutExercisesResponse
{
    public IEnumerable<ShortWorkoutExerciseResponse> WorkoutExercises { get; init; } = [];
    public int TotalCount { get; init; }
}
