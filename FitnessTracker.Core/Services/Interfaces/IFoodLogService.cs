using FitnessTracker.Core.Dtos.Requests.FoodLogs;
using FitnessTracker.Core.Dtos.Responses.FoodLogs;

namespace FitnessTracker.Core.Services.Interfaces;

public interface IFoodLogService
{
    Task<FoodLogsResponse> GetAllByUserAsync(Guid userId, CancellationToken token);

    Task AddAsync(AddFoodLogRequest request, Guid userId, CancellationToken token);
}
