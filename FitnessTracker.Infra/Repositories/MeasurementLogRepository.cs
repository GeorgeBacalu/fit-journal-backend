using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Repositories;

public class MeasurementLogRepository(FitnessTrackerContext context)
    : UserOwnedRepository<MeasurementLog>(context), IMeasurementLogRepository
{
    public async Task<IEnumerable<MeasurementLog>> GetByIdsAsync(IEnumerable<Guid> ids, Guid userId, CancellationToken token) =>
        await context.MeasurementLogs
            .Where(measurementLog => ids.Contains(measurementLog.Id) && measurementLog.UserId == userId)
            .ToListAsync(token);
}
