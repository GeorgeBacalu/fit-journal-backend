using FitnessTracker.Core.Dtos.Requests.FoodLogs;
using FitnessTracker.Core.Extensions.Pagination;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Repositories;

public class FoodLogRepository(AppDbContext db)
    : UserOwnedRepository<FoodLog>(db), IFoodLogRepository
{
    public IQueryable<FoodLog> GetAllBaseQuery(FoodLogPaginationRequest request, Guid? userId) =>
        _db.FoodLogs.AsNoTracking().Where(w => userId == null || w.UserId == userId).AddFilters(request);

    public IQueryable<FoodLog> GetAllQuery(FoodLogPaginationRequest request, Guid? userId) =>
        GetAllBaseQuery(request, userId).AddSorting(request).AddPaging(request);

    public async Task<int> RemoveRangeFoodItemsAsync(IEnumerable<Guid> foodItemIds, bool hardDelete, CancellationToken token)
    {
        var query = _db.FoodLogs.Where(fl => foodItemIds.Contains(fl.FoodId));

        return hardDelete
            ? await query.ExecuteDeleteAsync(token)
            : await query.ExecuteUpdateAsync(setter =>
                setter.SetProperty(fl => fl.DeletedAt, DateTime.UtcNow), token);
    }
}
