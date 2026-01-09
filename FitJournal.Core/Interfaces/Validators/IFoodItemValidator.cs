using FitJournal.Core.Dtos.Requests.FoodItems;
using FitJournal.Core.Interfaces.Services;

namespace FitJournal.Core.Interfaces.Validators;

public interface IFoodItemValidator : IBusinessService
{
    Task ValidateAddAsync(AddFoodItemRequest request, CancellationToken token);

    Task ValidateEditAsync(EditFoodItemRequest request, CancellationToken token);
}
