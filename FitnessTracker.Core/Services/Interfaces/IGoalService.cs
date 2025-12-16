using FitnessTracker.Core.Dtos.Requests.Goals;

namespace FitnessTracker.Core.Services.Interfaces;

public interface IGoalService
{
    Task AddAsync(AddGoalRequest request, CancellationToken token = default);
}
