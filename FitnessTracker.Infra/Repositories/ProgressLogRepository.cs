using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Repositories;

public class ProgressLogRepository(AppDbContext db)
    : UserOwnedRepository<ProgressLog>(db), IProgressLogRepository
{
    public async Task<ProgressLog?> GetLastAsync(Guid userId, CancellationToken token) =>
        await _db.ProgressLogs.AsNoTracking()
            .Where(pl => pl.UserId == userId)
            .OrderByDescending(pl => pl.Date)
            .FirstOrDefaultAsync(token);
}
