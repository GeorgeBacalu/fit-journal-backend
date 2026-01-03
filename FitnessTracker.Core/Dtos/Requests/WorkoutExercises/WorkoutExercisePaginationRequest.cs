using FitnessTracker.Core.Dtos.Requests.Pagination;
using FitnessTracker.Domain.Enums.Exercises;

namespace FitnessTracker.Core.Dtos.Requests.WorkoutExercises;

public record WorkoutExercisePaginationRequest : PaginationRequest
{
    public Guid WorkoutId { get; init; }

    public string? ExerciseSearchName { get; init; }
    public MuscleGroup? MuscleGroup { get; init; }
    public DifficultyLevel? DifficultyLevel { get; init; }

    public DateTime? WorkoutStartedAtFrom { get; init; }
    public DateTime? WorkoutStartedAtTo { get; init; }

    public decimal? WeightUsedFrom { get; init; }
    public decimal? WeightUsedTo { get; init; }

    public IEnumerable<WorkoutExerciseSort> Sort { get; init; } = [];
}

public record WorkoutExerciseSort(WorkoutExerciseSortField Field, SortDirection Direction);

public enum WorkoutExerciseSortField
{
    ExerciseName,
    MuscleGroup,
    DifficultyLevel,
    WorkoutStartedAt,
    WeightUsed
}
