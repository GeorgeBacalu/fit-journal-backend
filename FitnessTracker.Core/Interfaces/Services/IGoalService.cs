using FitnessTracker.Core.Dtos.Requests.Goals;
using FitnessTracker.Core.Dtos.Responses.Goals;

namespace FitnessTracker.Core.Interfaces.Services;

public interface IGoalService : IBusinessService
{
    Task<GoalsResponse> GetAllAsync(Guid userId, CancellationToken token);

    Task<GoalResponse> GetByIdAsync(Guid id, Guid userId, CancellationToken token);

    Task AddAsync(AddGoalRequest request, Guid userId, CancellationToken token);

    Task EditAsync(EditGoalRequest request, Guid userId, CancellationToken token);

    Task RemoveRangeAsync(RemoveGoalsRequest request, Guid userId, CancellationToken token);
}
