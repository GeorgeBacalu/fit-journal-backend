using FitJournal.Core.Dtos.Requests.FoodItems;
using FitJournal.Core.Dtos.Requests.Pagination;
using FitJournal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FitJournal.Core.Extensions.Pagination;

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
                FoodItemSortField.Name => Sort(ordered, query, fi => fi.Name, desc),
                FoodItemSortField.Category => Sort(ordered, query, fi => fi.Category, desc),
                FoodItemSortField.Brand => Sort(ordered, query, fi => fi.Brand, desc),
                FoodItemSortField.Calories => Sort(ordered, query, fi => fi.Calories, desc),
                FoodItemSortField.Protein => Sort(ordered, query, fi => fi.Protein, desc),
                FoodItemSortField.Carbs => Sort(ordered, query, fi => fi.Carbs, desc),
                FoodItemSortField.Fat => Sort(ordered, query, fi => fi.Fat, desc),
                _ => ordered
            };
        }

        return ordered ?? query.OrderBy(fi => fi.Name);
    }

    public static IQueryable<FoodItem> AddPaging(this IQueryable<FoodItem> query, FoodItemPaginationRequest request) =>
        query.Skip((request.Page - 1) * request.Size).Take(request.Size);

    private static IOrderedQueryable<FoodItem> Sort<TKey>(
        IOrderedQueryable<FoodItem>? ordered,
        IQueryable<FoodItem> query,
        Expression<Func<FoodItem, TKey>> key,
        bool desc) => ordered == null
            ? (desc ? query.OrderByDescending(key) : query.OrderBy(key))
            : (desc ? ordered.ThenByDescending(key) : ordered.ThenBy(key));
}
