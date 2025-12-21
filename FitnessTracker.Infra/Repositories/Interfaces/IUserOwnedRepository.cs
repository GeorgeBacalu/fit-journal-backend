using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Infra.Repositories.Interfaces;

public interface IUserOwnedRepository<T> : IBaseRepository<T> where T : BaseEntity, IUserOwnedEntity
{
    Task<IEnumerable<T>> GetAllAsync(Guid userId, CancellationToken token);

    IQueryable<T> GetAllQuery(Guid userId);

    Task<T?> GetByIdAsync(Guid id, Guid userId, CancellationToken token);

    Task<T?> GetByIdTrackedAsync(Guid id, Guid userId, CancellationToken token);

    Task<int> CountByIdsAsync(IEnumerable<Guid> ids, Guid userId, CancellationToken token);

    Task<int> RemoveRangeAsync(IEnumerable<Guid> ids, Guid userId, bool hardDelete, CancellationToken token);
}
