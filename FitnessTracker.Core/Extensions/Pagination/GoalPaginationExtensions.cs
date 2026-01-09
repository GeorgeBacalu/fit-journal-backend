using FitnessTracker.Core.Dtos.Requests.Goals;
using FitnessTracker.Core.Dtos.Requests.Pagination;
using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FitnessTracker.Core.Extensions.Pagination;

public static class GoalPaginationExtensions
{
    public static IQueryable<Goal> AddFilters(this IQueryable<Goal> query, GoalPaginationRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.SearchName))
            query = query.Where(g => EF.Functions.Like(g.Name, $"%{request.SearchName.Trim()}%"));

        if (request.Type != null) query = query.Where(g => g.Type == request.Type);
        if (request.IsAchieved != null) query = query.Where(g => g.IsAchieved == request.IsAchieved);
        if (request.DateFrom != null || request.DateTo != null)
            query = query.Where(g =>
                (request.DateFrom == null || g.StartDate >= request.DateFrom) &&
                (request.DateTo == null || g.EndDate <= request.DateTo));

        return query;
    }

    public static IQueryable<Goal> AddSorting(this IQueryable<Goal> query, GoalPaginationRequest request)
    {
        IOrderedQueryable<Goal>? ordered = null;

        foreach (var sort in request.Sort)
        {
            var desc = sort.Direction == SortDirection.Desc;

            ordered = sort.Field switch
            {
                GoalSortField.Name => Sort(ordered, query, g => g.Name, desc),
                GoalSortField.Type => Sort(ordered, query, g => g.Type, desc),
                GoalSortField.StartDate => Sort(ordered, query, g => g.StartDate, desc),
                _ => ordered
            };
        }

        return ordered ?? query.OrderBy(g => g.Name);
    }

    public static IQueryable<Goal> AddPaging(this IQueryable<Goal> query, GoalPaginationRequest request) =>
        query.Skip((request.Page - 1) * request.Size).Take(request.Size);

    private static IOrderedQueryable<Goal> Sort<TKey>(
        IOrderedQueryable<Goal>? ordered,
        IQueryable<Goal> query,
        Expression<Func<Goal, TKey>> key,
        bool desc) => ordered == null
            ? (desc ? query.OrderByDescending(key) : query.OrderBy(key))
            : (desc ? ordered.ThenByDescending(key) : ordered.ThenBy(key));
}
