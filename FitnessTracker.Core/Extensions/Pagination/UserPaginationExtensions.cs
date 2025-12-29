using FitnessTracker.Core.Dtos.Requests.Pagination;
using FitnessTracker.Core.Dtos.Requests.Users;
using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Core.Extensions.Pagination;

public static class UserPaginationExtensions
{
    public static IQueryable<User> AddFilters(this IQueryable<User> query, UserPaginationRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.SearchName))
            query = query.Where(u => EF.Functions.Like(u.Name, $"%{request.SearchName.Trim()}%"));

        if (request.Gender != null) query = query.Where(u => u.Gender == request.Gender);
        if (request.BirthdayFrom != null) query = query.Where(u => u.Birthday >= request.BirthdayFrom);
        if (request.BirthdayTo != null) query = query.Where(u => u.Birthday <= request.BirthdayTo);
        if (request.HeightFrom != null) query = query.Where(u => u.Height >= request.HeightFrom);
        if (request.HeightTo != null) query = query.Where(u => u.Height <= request.HeightTo);
        if (request.WeightFrom != null) query = query.Where(u => u.Weight >= request.WeightFrom);
        if (request.WeightTo != null) query = query.Where(u => u.Weight <= request.WeightTo);

        return query;
    }

    public static IQueryable<User> AddSorting(this IQueryable<User> query, UserPaginationRequest request)
    {
        IOrderedQueryable<User>? ordered = null;

        foreach (var sort in request.Sort)
        {
            var desc = sort.Direction == SortDirection.Desc;

            ordered = sort.Field switch
            {
                UserSortField.Name => ordered == null
                    ? (desc ? query.OrderByDescending(u => u.Name) : query.OrderBy(u => u.Name))
                    : (desc ? ordered.ThenByDescending(u => u.Name) : ordered.ThenBy(u => u.Name)),

                UserSortField.Birthday => ordered == null
                    ? (desc ? query.OrderByDescending(u => u.Birthday) : query.OrderBy(u => u.Birthday))
                    : (desc ? ordered.ThenByDescending(u => u.Birthday) : ordered.ThenBy(u => u.Birthday)),

                UserSortField.Height => ordered == null
                    ? (desc ? query.OrderByDescending(u => u.Height) : query.OrderBy(u => u.Height))
                    : (desc ? ordered.ThenByDescending(u => u.Height) : ordered.ThenBy(u => u.Height)),

                UserSortField.Weight => ordered == null
                    ? (desc ? query.OrderByDescending(u => u.Weight) : query.OrderBy(u => u.Weight))
                    : (desc ? ordered.ThenByDescending(u => u.Weight) : ordered.ThenBy(u => u.Weight)),

                _ => ordered
            };
        }

        return ordered ?? query.OrderBy(u => u.Name);
    }

    public static IQueryable<User> AddPaging(this IQueryable<User> query, UserPaginationRequest request) =>
        query.Skip((request.Page - 1) * request.Size).Take(request.Size);
}
