using FitnessTracker.Core.Dtos.Requests.Pagination;
using FitnessTracker.Domain.Enums.FoodItems;

namespace FitnessTracker.Core.Dtos.Requests.FoodItems;

public record FoodItemPaginationRequest : PaginationRequest
{
    public string? SearchName { get; init; }
    public FoodCategory? Category { get; init; }
    public FoodBrand? Brand { get; init; }

    public decimal? CaloriesFrom { get; init; }
    public decimal? CaloriesTo { get; init; }

    public decimal? ProteinFrom { get; init; }
    public decimal? ProteinTo { get; init; }

    public decimal? CarbsFrom { get; init; }
    public decimal? CarbsTo { get; init; }

    public decimal? FatFrom { get; init; }
    public decimal? FatTo { get; init; }

    public IEnumerable<FoodItemSort> Sort { get; init; } = [];
}

public record FoodItemSort(FoodItemSortField Field, SortDirection Direction);

public enum FoodItemSortField
{
    Name,
    Category,
    Brand,
    Calories,
    Protein,
    Carbs,
    Fat
}
