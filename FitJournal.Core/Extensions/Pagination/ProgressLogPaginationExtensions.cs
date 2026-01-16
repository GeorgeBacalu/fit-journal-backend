using FitJournal.Core.Dtos.Requests.Pagination;
using FitJournal.Core.Dtos.Requests.ProgressLogs;
using FitJournal.Domain.Entities;
using System.Linq.Expressions;

namespace FitJournal.Core.Extensions.Pagination;

public static class ProgressLogPaginationExtensions
{
    public static IQueryable<ProgressLog> AddFilters(this IQueryable<ProgressLog> query, ProgressLogPaginationRequest request)
    {
        if (request.DateFrom != null) query = query.Where(pl => pl.Date >= request.DateFrom);
        if (request.DateTo != null) query = query.Where(pl => pl.Date <= request.DateTo);

        return query;
    }

    public static IQueryable<ProgressLog> AddSorting(this IQueryable<ProgressLog> query, ProgressLogPaginationRequest request)
    {
        IOrderedQueryable<ProgressLog>? ordered = null;

        foreach (var sort in request.Sort)
        {
            var desc = sort.Direction == SortDirection.Desc;

            ordered = sort.Field switch
            {
                ProgressLogSortField.Date => Sort(ordered, query, pl => pl.Date, desc),
                _ => ordered
            };
        }

        return ordered ?? query.OrderBy(pl => pl.Date);
    }

    public static IQueryable<ProgressLog> AddPaging(this IQueryable<ProgressLog> query, ProgressLogPaginationRequest request) =>
        query.Skip((request.Page - 1) * request.Size).Take(request.Size);

    private static IOrderedQueryable<ProgressLog> Sort<TKey>(
        IOrderedQueryable<ProgressLog>? ordered,
        IQueryable<ProgressLog> query,
        Expression<Func<ProgressLog, TKey>> key,
        bool desc) => ordered == null
            ? (desc ? query.OrderByDescending(key) : query.OrderBy(key))
            : (desc ? ordered.ThenByDescending(key) : ordered.ThenBy(key));
}
