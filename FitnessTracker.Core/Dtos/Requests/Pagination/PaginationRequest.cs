namespace FitnessTracker.Core.Dtos.Requests.Pagination;

public record PaginationRequest
{
    public int Page { get; init; } = 1;
    public int Size { get; init; } = 10;
}

public enum SortDirection
{
    Asc,
    Desc
}
