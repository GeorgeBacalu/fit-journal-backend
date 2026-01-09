using FitJournal.Core.Dtos.Requests.Pagination;
using FitJournal.Core.Dtos.Requests.Workouts;
using FitJournal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FitJournal.Core.Extensions.Pagination;

public static class WorkoutPaginationExtensions
{
    public static IQueryable<Workout> AddFilters(this IQueryable<Workout> query, WorkoutPaginationRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.SearchName))
            query = query.Where(w => EF.Functions.Like(w.Name, $"%{request.SearchName.Trim()}%"));

        if (request.DurationMinutesFrom != null) query = query.Where(w => w.DurationMinutes >= request.DurationMinutesFrom);
        if (request.DurationMinutesTo != null) query = query.Where(w => w.DurationMinutes <= request.DurationMinutesTo);
        if (request.StartedAtFrom != null) query = query.Where(w => w.StartedAt >= request.StartedAtFrom);
        if (request.StartedAtTo != null) query = query.Where(w => w.StartedAt <= request.StartedAtTo);

        return query;
    }

    public static IQueryable<Workout> AddSorting(this IQueryable<Workout> query, WorkoutPaginationRequest request)
    {
        IOrderedQueryable<Workout>? ordered = null;

        foreach (var sort in request.Sort)
        {
            var desc = sort.Direction == SortDirection.Desc;

            ordered = sort.Field switch
            {
                WorkoutSortField.Name => Sort(ordered, query, w => w.Name, desc),
                WorkoutSortField.DurationMinutes => Sort(ordered, query, w => w.DurationMinutes, desc),
                WorkoutSortField.StartedAt => Sort(ordered, query, w => w.StartedAt, desc),
                _ => ordered
            };
        }

        return ordered ?? query.OrderBy(w => w.Name);
    }

    public static IQueryable<Workout> AddPaging(this IQueryable<Workout> query, WorkoutPaginationRequest request) =>
        query.Skip((request.Page - 1) * request.Size).Take(request.Size);

    private static IOrderedQueryable<Workout> Sort<TKey>(
        IOrderedQueryable<Workout>? ordered,
        IQueryable<Workout> query,
        Expression<Func<Workout, TKey>> key,
        bool desc) => ordered == null
            ? (desc ? query.OrderByDescending(key) : query.OrderBy(key))
            : (desc ? ordered.ThenByDescending(key) : ordered.ThenBy(key));
}
