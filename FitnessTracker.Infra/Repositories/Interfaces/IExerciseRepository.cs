using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Infra.Repositories.Interfaces;

public interface IExerciseRepository : IBaseRepository<Exercise>
{
    Task<IEnumerable<Exercise>> GetAllByIdsAsync(IEnumerable<Guid> ids, CancellationToken token = default);
}
