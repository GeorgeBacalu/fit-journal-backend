using FitnessTracker.Core.Dtos.Requests.Pagination;
using FitnessTracker.Core.Dtos.Requests.WorkoutExercises;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Domain.Enums.Exercises;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FitnessTracker.Core.Extensions.Pagination;

public static class WorkoutExercisePaginationExtensions
{
    public static IQueryable<WorkoutExercise> AddFilters(this IQueryable<WorkoutExercise> query, WorkoutExercisePaginationRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.ExerciseSearchName))
            query = query.Where(we => we.Exercise != null && EF.Functions.Like(we.Exercise.Name, $"%{request.ExerciseSearchName.Trim()}%"));

        if (request.MuscleGroup != null) query = query.Where(we => we.Exercise != null && we.Exercise.MuscleGroup == request.MuscleGroup);
        if (request.DifficultyLevel != null) query = query.Where(we => we.Exercise != null && we.Exercise.DifficultyLevel == request.DifficultyLevel);
        if (request.WorkoutStartedAtFrom != null) query = query.Where(we => we.Workout != null && we.Workout.StartedAt >= request.WorkoutStartedAtFrom);
        if (request.WorkoutStartedAtTo != null) query = query.Where(we => we.Workout != null && we.Workout.StartedAt <= request.WorkoutStartedAtTo);
        if (request.WeightUsedFrom != null) query = query.Where(we => we.WeightUsed >= request.WeightUsedFrom);
        if (request.WeightUsedTo != null) query = query.Where(we => we.WeightUsed <= request.WeightUsedTo);

        return query;
    }

    public static IQueryable<WorkoutExercise> AddSorting(this IQueryable<WorkoutExercise> query, WorkoutExercisePaginationRequest request)
    {
        IOrderedQueryable<WorkoutExercise>? ordered = null;

        foreach (var sort in request.Sort)
        {
            var desc = sort.Direction == SortDirection.Desc;

            ordered = sort.Field switch
            {
                WorkoutExerciseSortField.ExerciseName => Sort(ordered, query, we => we.Exercise != null ? we.Exercise.Name : string.Empty, desc),
                WorkoutExerciseSortField.MuscleGroup => Sort(ordered, query, we => we.Exercise != null ? we.Exercise.MuscleGroup : MuscleGroup.Unknown, desc),
                WorkoutExerciseSortField.DifficultyLevel => Sort(ordered, query, we => we.Exercise != null ? we.Exercise.DifficultyLevel : DifficultyLevel.Unknown, desc),
                WorkoutExerciseSortField.WorkoutStartedAt => Sort(ordered, query, we => we.Workout != null ? we.Workout.StartedAt : DateTime.MinValue, desc),
                WorkoutExerciseSortField.WeightUsed => Sort(ordered, query, we => we.WeightUsed, desc),
                _ => ordered
            };
        }

        return ordered ?? query.OrderBy(we => we.Exercise != null ? we.Exercise.Name : string.Empty);
    }

    public static IQueryable<WorkoutExercise> AddPaging(this IQueryable<WorkoutExercise> query, WorkoutExercisePaginationRequest request) =>
        query.Skip((request.Page - 1) * request.Size).Take(request.Size);

    private static IOrderedQueryable<WorkoutExercise> Sort<TKey>(
        IOrderedQueryable<WorkoutExercise>? ordered,
        IQueryable<WorkoutExercise> query,
        Expression<Func<WorkoutExercise, TKey>> key,
        bool desc) => ordered == null
            ? (desc ? query.OrderByDescending(key) : query.OrderBy(key))
            : (desc ? ordered.ThenByDescending(key) : ordered.ThenBy(key));
}
