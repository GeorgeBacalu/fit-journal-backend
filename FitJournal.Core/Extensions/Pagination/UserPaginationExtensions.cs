using FitJournal.Core.Dtos.Requests.Pagination;
using FitJournal.Core.Dtos.Requests.Users;
using FitJournal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FitJournal.Core.Extensions.Pagination;

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
                UserSortField.Name => Sort(ordered, query, u => u.Name, desc),
                UserSortField.Birthday => Sort(ordered, query, u => u.Birthday, desc),
                UserSortField.Height => Sort(ordered, query, u => u.Height, desc),
                UserSortField.Weight => Sort(ordered, query, u => u.Weight, desc),
                _ => ordered
            };
        }

        return ordered ?? query.OrderBy(u => u.Name);
    }

    public static IQueryable<User> AddPaging(this IQueryable<User> query, UserPaginationRequest request) =>
        query.Skip((request.Page - 1) * request.Size).Take(request.Size);

    private static IOrderedQueryable<User> Sort<TKey>(
        IOrderedQueryable<User>? ordered,
        IQueryable<User> query,
        Expression<Func<User, TKey>> key,
        bool desc) => ordered == null
            ? (desc ? query.OrderByDescending(key) : query.OrderBy(key))
            : (desc ? ordered.ThenByDescending(key) : ordered.ThenBy(key));
}
