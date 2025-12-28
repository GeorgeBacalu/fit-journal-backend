using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Core.Interfaces.Repositories;

public interface IProgressLogRepository : IUserOwnedRepository<ProgressLog>
{
    Task<ProgressLog?> GetLastAsync(Guid userId, CancellationToken token);
}
