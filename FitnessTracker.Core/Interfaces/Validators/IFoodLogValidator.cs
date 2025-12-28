using FitnessTracker.Core.Dtos.Requests.FoodLogs;
using FitnessTracker.Core.Interfaces.Services;

namespace FitnessTracker.Core.Interfaces.Validators;

public interface IFoodLogValidator : IBusinessService
{
    Task ValidateAddAsync(AddFoodLogRequest request, Guid userId, CancellationToken token);

    Task ValidateEditAsync(EditFoodLogRequest request, Guid userId, CancellationToken token);
}
