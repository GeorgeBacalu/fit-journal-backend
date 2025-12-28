using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Repositories;

public class WorkoutExerciseRepository(AppDbContext db)
    : BaseRepository<WorkoutExercise>(db), IWorkoutExerciseRepository
{
    public async Task<IEnumerable<WorkoutExercise>> GetAllAsync(Guid workoutId, CancellationToken token) =>
        await _db.WorkoutExercises.Where(we => we.WorkoutId == workoutId).ToListAsync(token);

    public IQueryable GetAllQuery(Guid workoutId) =>
        _db.WorkoutExercises.Where(we => we.WorkoutId == workoutId);

    public async Task<WorkoutExercise?> GetByIdAsync(Guid id, Guid userId, CancellationToken token) =>
        await _db.WorkoutExercises.FirstOrDefaultAsync(we => we.Workout != null && we.Workout.UserId == userId && we.Id == id, token);

    public async Task<bool> AnyInUseAsync(IEnumerable<Guid> ids, CancellationToken token) =>
        await _db.WorkoutExercises.AnyAsync(we => ids.Contains(we.ExerciseId), token);

    public async Task<int> CountByIdsAsync(IEnumerable<Guid> exerciseIds, Guid workoutId, CancellationToken token) =>
        await _db.WorkoutExercises.CountAsync(we => we.WorkoutId == workoutId && exerciseIds.Contains(we.ExerciseId), token);

    public async Task<int> RemoveRangeAsync(IEnumerable<Guid> exerciseIds, Guid workoutId, bool hardDelete, CancellationToken token)
    {
        var query = _db.WorkoutExercises.Where(we => we.WorkoutId == workoutId && exerciseIds.Contains(we.ExerciseId));

        return hardDelete
            ? await query.ExecuteDeleteAsync(token)
            : await query.ExecuteUpdateAsync(setter =>
                setter.SetProperty(we => we.DeletedAt, DateTime.UtcNow), token);
    }

    public async Task<int> RemoveRangeWorkoutsAsync(IEnumerable<Guid> workoutIds, bool hardDelete, CancellationToken token)
    {
        var query = _db.WorkoutExercises.Where(we => workoutIds.Contains(we.WorkoutId));

        return hardDelete
            ? await query.ExecuteDeleteAsync(token)
            : await query.ExecuteUpdateAsync(setter =>
                setter.SetProperty(we => we.DeletedAt, DateTime.UtcNow), token);
    }
}
