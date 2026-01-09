using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Domain.Entities;
using FitJournal.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FitJournal.Infra.Repositories;

public class BaseRepository<T>(AppDbContext db)
    : IBaseRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _db = db;

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken token) =>
        await _db.Set<T>().AsNoTracking().ToListAsync(token);

    public IQueryable<T> GetAllQuery() => _db.Set<T>().AsNoTracking();

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken token) =>
        await _db.Set<T>().AsNoTracking().FirstOrDefaultAsync(entity => entity.Id == id, token);

    public async Task<T?> GetByIdTrackedAsync(Guid id, CancellationToken token) =>
        await _db.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id, token);

    public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken token) =>
        await _db.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate, token);

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken token) =>
        await _db.Set<T>().AnyAsync(predicate, token);

    public async Task<int> CountByIdsAsync(IEnumerable<Guid> ids, CancellationToken token) =>
        await _db.Set<T>().CountAsync(entity => ids.Contains(entity.Id), token);

    public async Task AddAsync(T entity, CancellationToken token) =>
        await _db.Set<T>().AddAsync(entity, token);

    public async Task UpdateAsync(T entity, CancellationToken token) =>
        _db.Set<T>().Update(entity);

    public async Task RemoveAsync(T entity, bool hardDelete, CancellationToken token)
    {
        if (hardDelete)
            _db.Set<T>().Remove(entity);
        else
            entity.DeletedAt = DateTime.UtcNow;
    }

    public async Task<int> RemoveRangeAsync(IEnumerable<Guid> ids, bool hardDelete, CancellationToken token)
    {
        var query = _db.Set<T>().Where(entity => ids.Contains(entity.Id));

        return hardDelete
            ? await query.ExecuteDeleteAsync(token)
            : await query.ExecuteUpdateAsync(setter =>
                setter.SetProperty(entity => entity.DeletedAt, DateTime.UtcNow), token);
    }
}
