namespace FitnessTracker.Core.Dtos.Responses.WorkoutExercises;

public record WorkoutExercisesResponse
{
    public IEnumerable<ShortWorkoutExerciseResponse> WorkoutExercises { get; init; } = [];
    public int TotalCount { get; init; }
}
