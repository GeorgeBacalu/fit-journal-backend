using FitnessTracker.Core.Dtos.Common.WorkoutExercises;

namespace FitnessTracker.Core.Dtos.Responses.Workouts;

public record WorkoutResponse : ShortWorkoutResponse
{
    public string? Description { get; init; }
    public string? Notes { get; init; }

    public IEnumerable<WorkoutExerciseDto> WorkoutExercises { get; init; } = [];
}
