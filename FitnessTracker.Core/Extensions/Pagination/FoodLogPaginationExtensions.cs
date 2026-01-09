using FitnessTracker.Core.Dtos.Requests.FoodLogs;
using FitnessTracker.Core.Dtos.Requests.Pagination;
using FitnessTracker.Domain.Entities;
using System.Linq.Expressions;

namespace FitnessTracker.Core.Extensions.Pagination;

public static class FoodLogPaginationExtensions
{
    public static IQueryable<FoodLog> AddFilters(this IQueryable<FoodLog> query, FoodLogPaginationRequest request)
    {
        if (request.DateFrom != null) query = query.Where(fl => fl.Date >= request.DateFrom);
        if (request.DateTo != null) query = query.Where(fl => fl.Date <= request.DateTo);
        if (request.ServingsFrom != null) query = query.Where(fl => fl.Servings >= request.ServingsFrom);
        if (request.ServingsTo != null) query = query.Where(fl => fl.Servings <= request.ServingsTo);
        if (request.QuantityFrom != null) query = query.Where(fl => fl.Quantity >= request.QuantityFrom);
        if (request.QuantityTo != null) query = query.Where(fl => fl.Quantity <= request.QuantityTo);

        return query;
    }

    public static IQueryable<FoodLog> AddSorting(this IQueryable<FoodLog> query, FoodLogPaginationRequest request)
    {
        IOrderedQueryable<FoodLog>? ordered = null;

        foreach (var sort in request.Sort)
        {
            var desc = sort.Direction == SortDirection.Desc;

            ordered = sort.Field switch
            {
                FoodLogSortField.Date => Sort(ordered, query, fl => fl.Date, desc),
                FoodLogSortField.Servings => Sort(ordered, query, fl => fl.Servings, desc),
                FoodLogSortField.Quantity => Sort(ordered, query, fl => fl.Quantity, desc),
                _ => ordered
            };
        }

        return ordered ?? query.OrderBy(fl => fl.Date);
    }

    public static IQueryable<FoodLog> AddPaging(this IQueryable<FoodLog> query, FoodLogPaginationRequest request) =>
        query.Skip((request.Page - 1) * request.Size).Take(request.Size);

    private static IOrderedQueryable<FoodLog> Sort<TKey>(
        IOrderedQueryable<FoodLog>? ordered,
        IQueryable<FoodLog> query,
        Expression<Func<FoodLog, TKey>> key,
        bool desc) => ordered == null
            ? (desc ? query.OrderByDescending(key) : query.OrderBy(key))
            : (desc ? ordered.ThenByDescending(key) : ordered.ThenBy(key));
}
