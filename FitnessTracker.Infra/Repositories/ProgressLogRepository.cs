using FitnessTracker.Core.Dtos.Requests.ProgressLogs;
using FitnessTracker.Core.Extensions.Pagination;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Repositories;

public class ProgressLogRepository(AppDbContext db)
    : UserOwnedRepository<ProgressLog>(db), IProgressLogRepository
{
    public IQueryable<ProgressLog> GetAllBaseQuery(ProgressLogPaginationRequest request, Guid? userId) =>
        _db.ProgressLogs.AsNoTracking().Where(w => userId == null || w.UserId == userId).AddFilters(request);

    public IQueryable<ProgressLog> GetAllQuery(ProgressLogPaginationRequest request, Guid? userId) =>
        GetAllBaseQuery(request, userId).AddSorting(request).AddPaging(request);

    public async Task<ProgressLog?> GetLastAsync(Guid userId, CancellationToken token) =>
        await _db.ProgressLogs.AsNoTracking()
            .Where(pl => pl.UserId == userId)
            .OrderByDescending(pl => pl.Date)
            .FirstOrDefaultAsync(token);
}
