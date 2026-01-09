namespace FitnessTracker.Core.Dtos.Responses.WorkoutExercises;

public record UsersWorkoutExercisesResponse : IWorkoutExercisesResponse
{
    public IEnumerable<UserWorkoutExercisesResponse> Users { get; init; } = [];
    public int TotalCount { get; init; }
}
