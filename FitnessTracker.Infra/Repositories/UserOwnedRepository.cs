using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Repositories;

public class UserOwnedRepository<T>(AppDbContext db)
    : BaseRepository<T>(db), IUserOwnedRepository<T> where T : BaseEntity, IUserOwnedEntity
{
    public async Task<IEnumerable<T>> GetAllAsync(Guid userId, CancellationToken token) =>
        await _db.Set<T>().AsNoTracking().Where(entity => entity.UserId == userId).ToListAsync(token);

    public IQueryable<T> GetAllQuery(Guid userId) => _db.Set<T>().AsNoTracking().Where(entity => entity.UserId == userId);

    public async Task<T?> GetByIdAsync(Guid id, Guid userId, CancellationToken token) =>
        await _db.Set<T>().AsNoTracking().FirstOrDefaultAsync(entity => entity.UserId == userId && entity.Id == id, token);

    public async Task<T?> GetByIdTrackedAsync(Guid id, Guid userId, CancellationToken token) =>
        await _db.Set<T>().FirstOrDefaultAsync(entity => entity.UserId == userId && entity.Id == id, token);

    public async Task<int> CountByIdsAsync(IEnumerable<Guid> ids, Guid userId, CancellationToken token) =>
        await _db.Set<T>().CountAsync(entity => entity.UserId == userId && ids.Contains(entity.Id), token);

    public async Task<int> RemoveRangeAsync(IEnumerable<Guid> ids, Guid userId, bool hardDelete, CancellationToken token)
    {
        var query = _db.Set<T>().Where(entity => entity.UserId == userId && ids.Contains(entity.Id));

        return hardDelete
            ? await query.ExecuteDeleteAsync(token)
            : await query.ExecuteUpdateAsync(setter =>
                setter.SetProperty(entity => entity.DeletedAt, DateTime.UtcNow), token);
    }
}
