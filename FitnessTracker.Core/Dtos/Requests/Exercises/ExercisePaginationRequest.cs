using FitnessTracker.Core.Dtos.Requests.Pagination;
using FitnessTracker.Domain.Enums.Exercises;

namespace FitnessTracker.Core.Dtos.Requests.Exercises;

public record ExercisePaginationRequest : PaginationRequest
{
    public string? SearchName { get; init; }
    public MuscleGroup? MuscleGroup { get; init; }
    public DifficultyLevel? DifficultyLevel { get; init; }

    public IEnumerable<ExerciseSort> Sort { get; init; } = [];
}

public record ExerciseSort(ExerciseSortField Field, SortDirection Direction);

public enum ExerciseSortField
{
    Name,
    MuscleGroup,
    DifficultyLevel
}
