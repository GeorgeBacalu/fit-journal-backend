using FitJournal.Core.Dtos.Requests.FoodLogs;
using FitJournal.Core.Extensions.Pagination;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Domain.Entities;
using FitJournal.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FitJournal.Infra.Repositories;

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
