using FitnessTracker.Core.Dtos.Requests.FoodLogs;
using FitnessTracker.Core.Dtos.Responses.FoodLogs;

namespace FitnessTracker.Core.Interfaces.Services;

public interface IFoodLogService : IBusinessService
{
    Task<IFoodLogsResponse> GetAllAsync(FoodLogPaginationRequest request, Guid? userId, CancellationToken token);

    Task<FoodLogResponse> GetByIdAsync(Guid id, Guid? userId, CancellationToken token);

    Task AddAsync(AddFoodLogRequest request, Guid userId, CancellationToken token);

    Task EditAsync(EditFoodLogRequest request, Guid userId, CancellationToken token);

    Task RemoveRangeAsync(RemoveFoodLogsRequest request, Guid userId, CancellationToken token);
}
