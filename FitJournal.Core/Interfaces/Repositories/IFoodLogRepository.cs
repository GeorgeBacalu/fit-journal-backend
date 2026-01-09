using FitJournal.Core.Dtos.Requests.FoodLogs;
using FitJournal.Domain.Entities;

namespace FitJournal.Core.Interfaces.Repositories;

public interface IFoodLogRepository : IUserOwnedRepository<FoodLog>
{
    IQueryable<FoodLog> GetAllBaseQuery(FoodLogPaginationRequest request, Guid? userId);

    IQueryable<FoodLog> GetAllQuery(FoodLogPaginationRequest request, Guid? userId);

    Task<int> RemoveRangeFoodItemsAsync(IEnumerable<Guid> foodItemIds, bool hardDelete, CancellationToken token);
}
