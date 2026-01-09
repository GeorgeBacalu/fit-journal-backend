using FitJournal.Domain.Entities;
using System.Linq.Expressions;

namespace FitJournal.Core.Interfaces.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken token);

    IQueryable<T> GetAllQuery();

    Task<T?> GetByIdAsync(Guid id, CancellationToken token);

    Task<T?> GetByIdTrackedAsync(Guid id, CancellationToken token);

    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken token);

    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken token);

    Task<int> CountByIdsAsync(IEnumerable<Guid> ids, CancellationToken token);

    Task AddAsync(T entity, CancellationToken token);

    Task UpdateAsync(T entity, CancellationToken token);

    Task RemoveAsync(T entity, bool hardDelete, CancellationToken token);

    Task<int> RemoveRangeAsync(IEnumerable<Guid> ids, bool hardDelete, CancellationToken token);
}
