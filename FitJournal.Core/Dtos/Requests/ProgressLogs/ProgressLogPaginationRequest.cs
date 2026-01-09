using FitJournal.Core.Dtos.Requests.Pagination;

namespace FitJournal.Core.Dtos.Requests.ProgressLogs;

public record ProgressLogPaginationRequest : PaginationRequest
{
    public DateOnly? DateFrom { get; init; }
    public DateOnly? DateTo { get; init; }

    public IEnumerable<ProgressLogSort> Sort { get; init; } = [];
}

public record ProgressLogSort(ProgressLogSortField Field, SortDirection Direction);

public enum ProgressLogSortField
{
    Date
}
