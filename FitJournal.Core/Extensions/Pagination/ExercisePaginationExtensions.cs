using FitJournal.Core.Dtos.Requests.Exercises;
using FitJournal.Core.Dtos.Requests.Pagination;
using FitJournal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FitJournal.Core.Extensions.Pagination;

public static class ExercisePaginationExtensions
{
    public static IQueryable<Exercise> AddFilters(this IQueryable<Exercise> query, ExercisePaginationRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.SearchName))
            query = query.Where(e => EF.Functions.Like(e.Name, $"%{request.SearchName.Trim()}%"));

        if (request.MuscleGroup != null) query = query.Where(e => e.MuscleGroup == request.MuscleGroup);
        if (request.DifficultyLevel != null) query = query.Where(e => e.DifficultyLevel == request.DifficultyLevel);

        return query;
    }

    public static IQueryable<Exercise> AddSorting(this IQueryable<Exercise> query, ExercisePaginationRequest request)
    {
        IOrderedQueryable<Exercise>? ordered = null;

        foreach (var sort in request.Sort)
        {
            var desc = sort.Direction == SortDirection.Desc;

            ordered = sort.Field switch
            {
                ExerciseSortField.Name => Sort(ordered, query, e => e.Name, desc),
                ExerciseSortField.MuscleGroup => Sort(ordered, query, e => e.MuscleGroup, desc),
                ExerciseSortField.DifficultyLevel => Sort(ordered, query, e => e.DifficultyLevel, desc),
                _ => ordered
            };
        }

        return ordered ?? query.OrderBy(e => e.Name);
    }

    public static IQueryable<Exercise> AddPaging(this IQueryable<Exercise> query, ExercisePaginationRequest request) =>
        query.Skip((request.Page - 1) * request.Size).Take(request.Size);

    private static IOrderedQueryable<Exercise> Sort<TKey>(
        IOrderedQueryable<Exercise>? ordered,
        IQueryable<Exercise> query,
        Expression<Func<Exercise, TKey>> key,
        bool desc) => ordered == null
            ? (desc ? query.OrderByDescending(key) : query.OrderBy(key))
            : (desc ? ordered.ThenByDescending(key) : ordered.ThenBy(key));
}
