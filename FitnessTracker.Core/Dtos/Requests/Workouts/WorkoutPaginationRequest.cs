using FitnessTracker.Core.Dtos.Requests.Pagination;

namespace FitnessTracker.Core.Dtos.Requests.Workouts;

public record WorkoutPaginationRequest : PaginationRequest
{
    public string? SearchName { get; init; }

    public int? DurationMinutesFrom { get; init; }
    public int? DurationMinutesTo { get; init; }

    public DateTime? StartedAtFrom { get; init; }
    public DateTime? StartedAtTo { get; init; }

    public IEnumerable<WorkoutSort> Sort { get; init; } = [];
}

public record WorkoutSort(WorkoutSortField Field, SortDirection Direction);

public enum WorkoutSortField
{
    Name,
    DurationMinutes,
    StartedAt
}
