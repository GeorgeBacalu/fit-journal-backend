using FitnessTracker.Domain.Entities;
using System.Linq.Expressions;

namespace FitnessTracker.Infra.Repositories.Interfaces;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken token = default);
    Task<T?> GetByIdAsync(Guid id, CancellationToken token = default);
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default);
    Task AddAsync(T entity, CancellationToken token = default);
    Task UpdateAsync(T entity, CancellationToken token = default);
    Task RemoveAsync(T entity, bool hardDelete = false, CancellationToken token = default);
    Task RemoveRangeAsync(IEnumerable<T> entities, bool hardDelete = false, CancellationToken token = default);
}
