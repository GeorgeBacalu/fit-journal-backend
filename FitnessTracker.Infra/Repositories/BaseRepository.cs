using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Repositories;

public class BaseRepository<T>(FitnessTrackerContext context)
    : IBaseRepository<T> where T : BaseEntity
{
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken token = default) =>
        await context.Set<T>().AsNoTracking().ToListAsync(token);

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken token = default) =>
        await context.Set<T>().SingleOrDefaultAsync(entity => entity.Id == id, token);

    public async Task AddAsync(T entity, CancellationToken token = default) =>
        await context.Set<T>().AddAsync(entity, token);

    public async Task UpdateAsync(T entity, CancellationToken token = default) =>
        context.Set<T>().Update(entity);

    public async Task RemoveAsync(T entity, bool hardDelete = false, CancellationToken token = default)
    {
        if (hardDelete)
            context.Set<T>().Remove(entity);
        else
            entity.DeletedAt = DateTime.UtcNow;
    }

    public async Task RemoveRangeAsync(IEnumerable<T> entities, bool hardDelete = false, CancellationToken token = default)
    {
        if (hardDelete)
            context.Set<T>().RemoveRange(entities);
        else
            foreach (var entity in entities)
                entity.DeletedAt = DateTime.UtcNow;
    }
}
