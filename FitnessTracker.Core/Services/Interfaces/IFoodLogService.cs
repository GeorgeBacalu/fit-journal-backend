using FitnessTracker.Core.Dtos.Requests.FoodLogs;

namespace FitnessTracker.Core.Services.Interfaces;

public interface IFoodLogService
{
    Task AddAsync(AddFoodLogRequest request, Guid userId, CancellationToken token);
}
