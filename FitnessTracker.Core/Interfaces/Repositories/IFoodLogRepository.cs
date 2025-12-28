using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Core.Interfaces.Repositories;

public interface IFoodLogRepository : IUserOwnedRepository<FoodLog>
{
    Task<int> RemoveRangeFoodItemsAsync(IEnumerable<Guid> foodItemIds, bool hardDelete, CancellationToken token);
}
