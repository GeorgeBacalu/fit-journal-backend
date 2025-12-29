using FitnessTracker.Core.Dtos.Requests.Goals;
using FitnessTracker.Core.Interfaces.Services;

namespace FitnessTracker.Core.Interfaces.Validators;

public interface IGoalValidator : IBusinessService
{
    Task ValidateAddAsync(AddGoalRequest request, Guid userId, CancellationToken token);

    Task ValidateEditAsync(EditGoalRequest request, Guid userId, CancellationToken token);
}
