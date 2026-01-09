using FitJournal.Core.Dtos.Requests.Pagination;
using FitJournal.Domain.Enums.Exercises;

namespace FitJournal.Core.Dtos.Requests.Exercises;

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
