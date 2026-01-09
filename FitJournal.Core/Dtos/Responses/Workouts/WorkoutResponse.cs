using FitJournal.Core.Dtos.Common.WorkoutExercises;

namespace FitJournal.Core.Dtos.Responses.Workouts;

public record WorkoutResponse : ShortWorkoutResponse
{
    public string? Description { get; init; }
    public string? Notes { get; init; }

    public IEnumerable<WorkoutExerciseDto> WorkoutExercises { get; init; } = [];
}
