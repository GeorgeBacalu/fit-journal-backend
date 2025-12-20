using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Infra.Repositories.Interfaces;

public interface IExerciseRepository : IBaseRepository<Exercise>
{
    Task<bool> AnyInUseAsync(IEnumerable<Guid> ids, CancellationToken token);
}
