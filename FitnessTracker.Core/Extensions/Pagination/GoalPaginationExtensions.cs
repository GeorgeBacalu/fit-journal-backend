using FitnessTracker.Core.Dtos.Requests.Goals;
using FitnessTracker.Core.Dtos.Requests.Pagination;
using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
                GoalSortField.Name => ordered == null
                    ? (desc ? query.OrderByDescending(g => g.Name) : query.OrderBy(g => g.Name))
                    : (desc ? ordered.ThenByDescending(g => g.Name) : ordered.ThenBy(g => g.Name)),

                GoalSortField.Type => ordered == null
                    ? (desc ? query.OrderByDescending(g => g.Type) : query.OrderBy(g => g.Type))
                    : (desc ? ordered.ThenByDescending(g => g.Type) : ordered.ThenBy(g => g.Type)),

                GoalSortField.StartDate => ordered == null
                    ? (desc ? query.OrderByDescending(g => g.StartDate) : query.OrderBy(g => g.StartDate))
                    : (desc ? ordered.ThenByDescending(g => g.StartDate) : ordered.ThenBy(g => g.StartDate)),

                _ => ordered
            };
        }

        return ordered ?? query.OrderBy(g => g.Name);
    }

    public static IQueryable<Goal> AddPaging(this IQueryable<Goal> query, GoalPaginationRequest request) =>
        query.Skip((request.Page - 1) * request.Size).Take(request.Size);
}
