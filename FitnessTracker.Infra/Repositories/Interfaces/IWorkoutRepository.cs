using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Infra.Repositories.Interfaces;

public interface IWorkoutRepository : IBaseRepository<Workout>
{
    Task<IEnumerable<Workout>> GetAllByIdsAsync(IEnumerable<Guid> ids, CancellationToken token = default);
}
