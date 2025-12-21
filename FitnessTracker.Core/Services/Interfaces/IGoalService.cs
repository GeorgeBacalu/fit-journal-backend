using FitnessTracker.Core.Dtos.Requests.Goals;
using FitnessTracker.Core.Dtos.Responses.Goals;

namespace FitnessTracker.Core.Services.Interfaces;

public interface IGoalService
{
    Task<GoalsResponse> GetAllByUserAsync(Guid userId, CancellationToken token);

    Task<GoalResponse> GetByIdAsync(Guid id, CancellationToken token);

    Task AddAsync(AddGoalRequest request, Guid userId, CancellationToken token);

    Task EditAsync(EditGoalRequest request, Guid userId, CancellationToken token);

    Task RemoveRangeAsync(RemoveGoalsRequest request, Guid userId, CancellationToken token);
}
