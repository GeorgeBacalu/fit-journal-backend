using FitnessTracker.Core.Dtos.Requests.FoodItems;
using FitnessTracker.Core.Dtos.Requests.Pagination;
using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Core.Extensions.Pagination;

public static class FoodItemPaginationExtensions
{
    public static IQueryable<FoodItem> AddFilters(this IQueryable<FoodItem> query, FoodItemPaginationRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.SearchName))
            query = query.Where(fi => EF.Functions.Like(fi.Name, $"%{request.SearchName.Trim()}%"));

        if (request.Category != null) query = query.Where(fi => fi.Category == request.Category);
        if (request.Brand != null) query = query.Where(fi => fi.Brand == request.Brand);
        if (request.CaloriesFrom != null) query = query.Where(fi => fi.Calories >= request.CaloriesFrom);
        if (request.CaloriesTo != null) query = query.Where(fi => fi.Calories <= request.CaloriesTo);
        if (request.ProteinFrom != null) query = query.Where(fi => fi.Protein >= request.ProteinFrom);
        if (request.ProteinTo != null) query = query.Where(fi => fi.Protein <= request.ProteinTo);
        if (request.CarbsFrom != null) query = query.Where(fi => fi.Carbs >= request.CarbsFrom);
        if (request.CarbsTo != null) query = query.Where(fi => fi.Carbs <= request.CarbsTo);
        if (request.FatFrom != null) query = query.Where(fi => fi.Fat >= request.FatFrom);
        if (request.FatTo != null) query = query.Where(fi => fi.Fat <= request.FatTo);

        return query;
    }

    public static IQueryable<FoodItem> AddSorting(this IQueryable<FoodItem> query, FoodItemPaginationRequest request)
    {
        IOrderedQueryable<FoodItem>? ordered = null;

        foreach (var sort in request.Sort)
        {
            var desc = sort.Direction == SortDirection.Desc;

            ordered = sort.Field switch
            {
                FoodItemSortField.Name => ordered == null
                    ? (desc ? query.OrderByDescending(fi => fi.Name) : query.OrderBy(fi => fi.Name))
                    : (desc ? ordered.ThenByDescending(fi => fi.Name) : ordered.ThenBy(fi => fi.Name)),

                FoodItemSortField.Category => ordered == null
                    ? (desc ? query.OrderByDescending(fi => fi.Category) : query.OrderBy(fi => fi.Category))
                    : (desc ? ordered.ThenByDescending(fi => fi.Category) : ordered.ThenBy(fi => fi.Category)),

                FoodItemSortField.Brand => ordered == null
                    ? (desc ? query.OrderByDescending(fi => fi.Brand) : query.OrderBy(fi => fi.Brand))
                    : (desc ? ordered.ThenByDescending(fi => fi.Brand) : ordered.ThenBy(fi => fi.Brand)),

                FoodItemSortField.Calories => ordered == null
                    ? (desc ? query.OrderByDescending(fi => fi.Calories) : query.OrderBy(fi => fi.Calories))
                    : (desc ? ordered.ThenByDescending(fi => fi.Calories) : ordered.ThenBy(fi => fi.Calories)),

                FoodItemSortField.Protein => ordered == null
                    ? (desc ? query.OrderByDescending(fi => fi.Protein) : query.OrderBy(fi => fi.Protein))
                    : (desc ? ordered.ThenByDescending(fi => fi.Protein) : ordered.ThenBy(fi => fi.Protein)),

                FoodItemSortField.Carbs => ordered == null
                    ? (desc ? query.OrderByDescending(fi => fi.Carbs) : query.OrderBy(fi => fi.Carbs))
                    : (desc ? ordered.ThenByDescending(fi => fi.Carbs) : ordered.ThenBy(fi => fi.Carbs)),

                FoodItemSortField.Fat => ordered == null
                    ? (desc ? query.OrderByDescending(fi => fi.Fat) : query.OrderBy(fi => fi.Fat))
                    : (desc ? ordered.ThenByDescending(fi => fi.Fat) : ordered.ThenBy(fi => fi.Fat)),

                _ => ordered
            };
        }

        return ordered ?? query.OrderBy(fi => fi.Name);
    }

    public static IQueryable<FoodItem> AddPaging(this IQueryable<FoodItem> query, FoodItemPaginationRequest request) =>
        query.Skip((request.Page - 1) * request.Size).Take(request.Size);
}
