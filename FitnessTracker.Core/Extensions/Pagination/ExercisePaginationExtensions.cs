using FitnessTracker.Core.Dtos.Requests.Exercises;
using FitnessTracker.Core.Dtos.Requests.Pagination;
using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Core.Extensions.Pagination;

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
                ExerciseSortField.Name => ordered == null
                    ? (desc ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name))
                    : (desc ? ordered.ThenByDescending(e => e.Name) : ordered.ThenBy(e => e.Name)),

                ExerciseSortField.MuscleGroup => ordered == null
                    ? (desc ? query.OrderByDescending(e => e.MuscleGroup) : query.OrderBy(e => e.MuscleGroup))
                    : (desc ? ordered.ThenByDescending(e => e.MuscleGroup) : ordered.ThenBy(e => e.MuscleGroup)),

                ExerciseSortField.DifficultyLevel => ordered == null
                    ? (desc ? query.OrderByDescending(e => e.DifficultyLevel) : query.OrderBy(e => e.DifficultyLevel))
                    : (desc ? ordered.ThenByDescending(e => e.DifficultyLevel) : ordered.ThenBy(e => e.DifficultyLevel)),

                _ => ordered
            };
        }

        return ordered ?? query.OrderBy(e => e.Name);
    }

    public static IQueryable<Exercise> AddPaging(this IQueryable<Exercise> query, ExercisePaginationRequest request) =>
        query.Skip((request.Page - 1) * request.Size).Take(request.Size);
}
