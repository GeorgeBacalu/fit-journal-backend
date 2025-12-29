using FitnessTracker.Core.Dtos.Requests.Pagination;
using FitnessTracker.Core.Dtos.Requests.Workouts;
using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Core.Extensions.Pagination;

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
                WorkoutSortField.Name => ordered == null
                    ? (desc ? query.OrderByDescending(w => w.Name) : query.OrderBy(w => w.Name))
                    : (desc ? ordered.ThenByDescending(w => w.Name) : ordered.ThenBy(w => w.Name)),

                WorkoutSortField.DurationMinutes => ordered == null
                    ? (desc ? query.OrderByDescending(w => w.DurationMinutes) : query.OrderBy(w => w.DurationMinutes))
                    : (desc ? ordered.ThenByDescending(w => w.DurationMinutes) : ordered.ThenBy(w => w.DurationMinutes)),

                WorkoutSortField.StartedAt => ordered == null
                    ? (desc ? query.OrderByDescending(w => w.StartedAt) : query.OrderBy(w => w.StartedAt))
                    : (desc ? ordered.ThenByDescending(w => w.StartedAt) : ordered.ThenBy(w => w.StartedAt)),

                _ => ordered
            };
        }

        return ordered ?? query.OrderBy(w => w.Name);
    }

    public static IQueryable<Workout> AddPaging(this IQueryable<Workout> query, WorkoutPaginationRequest request) =>
        query.Skip((request.Page - 1) * request.Size).Take(request.Size);
}
