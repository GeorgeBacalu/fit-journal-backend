using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Infra.Repositories.Interfaces;

public interface IMeasurementLogRepository : IUserOwnedRepository<MeasurementLog>
{
    Task<IEnumerable<MeasurementLog>> GetByIdsAsync(IEnumerable<Guid> ids, Guid userId, CancellationToken token);
}
