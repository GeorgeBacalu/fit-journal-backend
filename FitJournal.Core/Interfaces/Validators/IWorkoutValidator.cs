using FitJournal.Core.Dtos.Requests.Workouts;
using FitJournal.Core.Interfaces.Services;

namespace FitJournal.Core.Interfaces.Validators;

public interface IWorkoutValidator : IBusinessService
{
    Task ValidateAddAsync(AddWorkoutRequest request, Guid userId, CancellationToken token);

    Task ValidateEditAsync(EditWorkoutRequest request, Guid userId, CancellationToken token);
}
