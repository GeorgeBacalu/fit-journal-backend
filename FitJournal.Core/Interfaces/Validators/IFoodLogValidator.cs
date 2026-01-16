using FitJournal.Core.Dtos.Requests.FoodLogs;
using FitJournal.Core.Interfaces.Services;

namespace FitJournal.Core.Interfaces.Validators;

public interface IFoodLogValidator : IBusinessService
{
    Task ValidateAddAsync(AddFoodLogRequest request, Guid userId, CancellationToken token);

    Task ValidateEditAsync(EditFoodLogRequest request, Guid userId, CancellationToken token);
}
