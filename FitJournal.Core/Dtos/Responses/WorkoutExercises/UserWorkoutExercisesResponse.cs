namespace FitJournal.Core.Dtos.Responses.WorkoutExercises;

public record UserWorkoutExercisesResponse
{
    public Guid? UserId { get; init; }
    public string? UserName { get; init; }
    public IEnumerable<ShortWorkoutExerciseResponse> WorkoutExercises { get; init; } = [];
}
