using FitnessTracker.Core.Dtos.Requests.Pagination;
using FitnessTracker.Domain.Enums.Users;

namespace FitnessTracker.Core.Dtos.Requests.Users;

public record UserPaginationRequest : PaginationRequest
{
    public string? SearchName { get; init; }
    public Gender? Gender { get; init; }

    public DateOnly? BirthdayFrom { get; init; }
    public DateOnly? BirthdayTo { get; init; }

    public decimal? HeightFrom { get; init; }
    public decimal? HeightTo { get; init; }

    public decimal? WeightFrom { get; init; }
    public decimal? WeightTo { get; init; }

    public IEnumerable<UserSort> Sort { get; init; } = [];
}

public record UserSort(UserSortField Field, SortDirection Direction);

public enum UserSortField
{
    Name,
    Birthday,
    Height,
    Weight
}
