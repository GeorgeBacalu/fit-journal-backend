using FitnessTracker.Core.Dtos.Requests.Exercises;
using FitnessTracker.Core.Interfaces.Services;

namespace FitnessTracker.Core.Interfaces.Validators;

public interface IExerciseValidator : IBusinessService
{
    Task ValidateAddAsync(AddExerciseRequest request, CancellationToken token);

    Task ValidateEditAsync(EditExerciseRequest request, CancellationToken token);
}
