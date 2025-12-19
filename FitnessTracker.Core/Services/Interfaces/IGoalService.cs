using FitnessTracker.Core.Dtos.Requests.Goals;
using FitnessTracker.Core.Dtos.Responses.Goals;

namespace FitnessTracker.Core.Services.Interfaces;

public interface IGoalService
{
    Task<GoalsResponse> GetAllByUserAsync(Guid userId, bool isAchieved = false, CancellationToken token = default);
    Task AddAsync(AddGoalRequest request, Guid userId, CancellationToken token = default);
}
