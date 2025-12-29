using FitnessTracker.Core.Dtos.Requests.Pagination;
using FitnessTracker.Core.Dtos.Requests.ProgressLogs;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Core.Extensions.Pagination;

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
                ProgressLogSortField.Date => ordered == null
                    ? (desc ? query.OrderByDescending(pl => pl.Date) : query.OrderBy(pl => pl.Date))
                    : (desc ? ordered.ThenByDescending(pl => pl.Date) : ordered.ThenBy(pl => pl.Date)),

                _ => ordered
            };
        }

        return ordered ?? query.OrderBy(pl => pl.Date);
    }

    public static IQueryable<ProgressLog> AddPaging(this IQueryable<ProgressLog> query, ProgressLogPaginationRequest request) =>
        query.Skip((request.Page - 1) * request.Size).Take(request.Size);
}
