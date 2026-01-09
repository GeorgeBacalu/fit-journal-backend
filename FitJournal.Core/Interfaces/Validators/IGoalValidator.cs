using FitJournal.Core.Dtos.Requests.Goals;
using FitJournal.Core.Interfaces.Services;

namespace FitJournal.Core.Interfaces.Validators;

public interface IGoalValidator : IBusinessService
{
    Task ValidateAddAsync(AddGoalRequest request, Guid userId, CancellationToken token);

    Task ValidateEditAsync(EditGoalRequest request, Guid userId, CancellationToken token);
}
