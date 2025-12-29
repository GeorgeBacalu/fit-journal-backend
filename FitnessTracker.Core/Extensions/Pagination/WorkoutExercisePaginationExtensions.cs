using FitnessTracker.Core.Dtos.Requests.Pagination;
using FitnessTracker.Core.Dtos.Requests.WorkoutExercises;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;

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
                WorkoutExerciseSortField.ExerciseName => ordered == null
                    ? (desc ? query.OrderByDescending(we => we.Exercise != null ? we.Exercise.Name : string.Empty)
                            : query.OrderBy(we => we.Exercise != null ? we.Exercise.Name : string.Empty))
                    : (desc ? ordered.ThenByDescending(we => we.Exercise != null ? we.Exercise.Name : string.Empty)
                            : ordered.ThenBy(we => we.Exercise != null ? we.Exercise.Name : string.Empty)),

                WorkoutExerciseSortField.MuscleGroup => ordered == null
                    ? (desc ? query.OrderByDescending(we => we.Exercise != null ? we.Exercise.MuscleGroup : MuscleGroup.Unknown)
                            : query.OrderBy(we => we.Exercise != null ? we.Exercise.MuscleGroup : MuscleGroup.Unknown))
                    : (desc ? ordered.ThenByDescending(we => we.Exercise != null ? we.Exercise.MuscleGroup : MuscleGroup.Unknown)
                            : ordered.ThenBy(we => we.Exercise != null ? we.Exercise.MuscleGroup : MuscleGroup.Unknown)),

                WorkoutExerciseSortField.DifficultyLevel => ordered == null
                    ? (desc ? query.OrderByDescending(we => we.Exercise != null ? we.Exercise.DifficultyLevel : DifficultyLevel.Unknown)
                            : query.OrderBy(we => we.Exercise != null ? we.Exercise.DifficultyLevel : DifficultyLevel.Unknown))
                    : (desc ? ordered.ThenByDescending(we => we.Exercise != null ? we.Exercise.DifficultyLevel : DifficultyLevel.Unknown)
                            : ordered.ThenBy(we => we.Exercise != null ? we.Exercise.DifficultyLevel : DifficultyLevel.Unknown)),

                WorkoutExerciseSortField.WorkoutStartedAt => ordered == null
                    ? (desc ? query.OrderByDescending(we => we.Workout != null ? we.Workout.StartedAt : DateTime.MinValue)
                            : query.OrderBy(we => we.Workout != null ? we.Workout.StartedAt : DateTime.MinValue))
                    : (desc ? ordered.ThenByDescending(we => we.Workout != null ? we.Workout.StartedAt : DateTime.MinValue)
                            : ordered.ThenBy(we => we.Workout != null ? we.Workout.StartedAt : DateTime.MinValue)),

                WorkoutExerciseSortField.WeightUsed => ordered == null
                    ? (desc ? query.OrderByDescending(we => we.WeightUsed) : query.OrderBy(we => we.WeightUsed))
                    : (desc ? ordered.ThenByDescending(we => we.WeightUsed) : ordered.ThenBy(we => we.WeightUsed)),

                _ => ordered
            };
        }

        return ordered ?? query.OrderBy(we => we.Exercise != null ? we.Exercise.Name : string.Empty);
    }

    public static IQueryable<WorkoutExercise> AddPaging(this IQueryable<WorkoutExercise> query, WorkoutExercisePaginationRequest request) =>
        query.Skip((request.Page - 1) * request.Size).Take(request.Size);
}
