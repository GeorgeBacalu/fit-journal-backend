using FitJournal.Core.Dtos.Requests.Pagination;

namespace FitJournal.Core.Dtos.Requests.FoodLogs;

public record FoodLogPaginationRequest : PaginationRequest
{
    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }

    public int? ServingsFrom { get; init; }
    public int? ServingsTo { get; init; }

    public decimal? QuantityFrom { get; init; }
    public decimal? QuantityTo { get; init; }

    public IEnumerable<FoodLogSort> Sort { get; init; } = [];
}

public record FoodLogSort(FoodLogSortField Field, SortDirection Direction);

public enum FoodLogSortField
{
    Date,
    Servings,
    Quantity
}
