using FitJournal.Core.Dtos.Requests.Exercises;
using FitJournal.Core.Interfaces.Services;

namespace FitJournal.Core.Interfaces.Validators;

public interface IExerciseValidator : IBusinessService
{
    Task ValidateAddAsync(AddExerciseRequest request, CancellationToken token);

    Task ValidateEditAsync(EditExerciseRequest request, CancellationToken token);
}
