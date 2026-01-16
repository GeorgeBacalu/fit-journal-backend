using FitJournal.Core.Dtos.Requests.Pagination;
using FitJournal.Domain.Enums.Goals;

namespace FitJournal.Core.Dtos.Requests.Goals;

public record GoalPaginationRequest : PaginationRequest
{
    public string? SearchName { get; init; }
    public GoalType? Type { get; init; }
    public bool? IsAchieved { get; init; }

    public DateOnly? DateFrom { get; init; }
    public DateOnly? DateTo { get; init; }

    public IEnumerable<GoalSort> Sort { get; init; } = [];
}

public record GoalSort(GoalSortField Field, SortDirection Direction);

public enum GoalSortField
{
    Name,
    Type,
    StartDate
}
