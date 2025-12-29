using FitnessTracker.Core.Dtos.Requests.FoodLogs;
using FitnessTracker.Core.Dtos.Requests.Pagination;
using FitnessTracker.Domain.Entities;

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
                FoodLogSortField.Date => ordered == null
                    ? (desc ? query.OrderByDescending(fl => fl.Date) : query.OrderBy(fl => fl.Date))
                    : (desc ? ordered.ThenByDescending(fl => fl.Date) : ordered.ThenBy(fl => fl.Date)),

                FoodLogSortField.Servings => ordered == null
                    ? (desc ? query.OrderByDescending(fl => fl.Servings) : query.OrderBy(fl => fl.Servings))
                    : (desc ? ordered.ThenByDescending(fl => fl.Servings) : ordered.ThenBy(fl => fl.Servings)),

                FoodLogSortField.Quantity => ordered == null
                    ? (desc ? query.OrderByDescending(fl => fl.Quantity) : query.OrderBy(fl => fl.Quantity))
                    : (desc ? ordered.ThenByDescending(fl => fl.Quantity) : ordered.ThenBy(fl => fl.Quantity)),
                _ => ordered
            };
        }

        return ordered ?? query.OrderBy(fl => fl.Date);
    }

    public static IQueryable<FoodLog> AddPaging(this IQueryable<FoodLog> query, FoodLogPaginationRequest request) =>
        query.Skip((request.Page - 1) * request.Size).Take(request.Size);
}
