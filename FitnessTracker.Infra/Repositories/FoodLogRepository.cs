using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Repositories;

public class FoodLogRepository(AppDbContext db)
    : UserOwnedRepository<FoodLog>(db), IFoodLogRepository
{
    public async Task<int> RemoveRangeFoodItemsAsync(IEnumerable<Guid> foodItemIds, bool hardDelete, CancellationToken token)
    {
        var query = _db.FoodLogs.Where(fl => foodItemIds.Contains(fl.FoodId));

        return hardDelete
            ? await query.ExecuteDeleteAsync(token)
            : await query.ExecuteUpdateAsync(setter =>
                setter.SetProperty(fl => fl.DeletedAt, DateTime.UtcNow), token);
    }
}
