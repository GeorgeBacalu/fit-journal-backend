using FitnessTracker.Core.Dtos.Requests.Workouts;
using FitnessTracker.Core.Interfaces.Services;

namespace FitnessTracker.Core.Interfaces.Validators;

public interface IWorkoutValidator : IBusinessService
{
    Task ValidateAddAsync(AddWorkoutRequest request, Guid userId, CancellationToken token);

    Task ValidateEditAsync(EditWorkoutRequest request, Guid userId, CancellationToken token);
}
