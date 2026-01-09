using FitJournal.Core.Dtos.Requests.Goals;
using FitJournal.Core.Dtos.Responses.Goals;

namespace FitJournal.Core.Interfaces.Services;

public interface IGoalService : IBusinessService
{
    Task<IGoalsResponse> GetAllAsync(GoalPaginationRequest request, Guid? userId, CancellationToken token);

    Task<GoalResponse> GetByIdAsync(Guid id, Guid? userId, CancellationToken token);

    Task AddAsync(AddGoalRequest request, Guid userId, CancellationToken token);

    Task EditAsync(EditGoalRequest request, Guid userId, CancellationToken token);

    Task RemoveRangeAsync(RemoveGoalsRequest request, Guid userId, CancellationToken token);
}
