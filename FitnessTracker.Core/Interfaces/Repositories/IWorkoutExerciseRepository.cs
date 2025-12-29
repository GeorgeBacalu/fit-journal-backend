using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Core.Interfaces.Repositories;

public interface IWorkoutExerciseRepository : IBaseRepository<WorkoutExercise>
{
    Task<IEnumerable<WorkoutExercise>> GetAllAsync(Guid workoutId, CancellationToken token);

    IQueryable<WorkoutExercise> GetAllQuery(Guid workoutId);

    Task<WorkoutExercise?> GetByIdAsync(Guid id, Guid userId, CancellationToken token);

    Task<bool> AnyInUseAsync(IEnumerable<Guid> ids, CancellationToken token);

    Task<int> CountByIdsAsync(IEnumerable<Guid> exerciseIds, Guid workoutId, CancellationToken token);

    Task<int> RemoveRangeAsync(IEnumerable<Guid> exerciseIds, Guid workoutId, bool hardDelete, CancellationToken token);

    Task<int> RemoveRangeWorkoutsAsync(IEnumerable<Guid> workoutIds, bool hardDelete, CancellationToken token);
}
