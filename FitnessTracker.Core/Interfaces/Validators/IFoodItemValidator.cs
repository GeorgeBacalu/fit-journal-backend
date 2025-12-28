using FitnessTracker.Core.Dtos.Requests.FoodItems;
using FitnessTracker.Core.Interfaces.Services;

namespace FitnessTracker.Core.Interfaces.Validators;

public interface IFoodItemValidator : IBusinessService
{
    Task ValidateAddAsync(AddFoodItemRequest request, CancellationToken token);

    Task ValidateEditAsync(EditFoodItemRequest request, CancellationToken token);
}
