using FitnessTracker.Core.Dtos.Requests.Exercises;
using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Validators.Exercises;

public class RemoveExercisesValidator : AbstractValidator<RemoveExercisesRequest>
{
    public RemoveExercisesValidator()
    {
        RuleFor(request => request.Ids)
            .NotEmpty()
            .WithMessage(ValidationErrors.ExerciseIdsRequired)

            .Must(ids => ids.Distinct().Count() == ids.Count())
            .WithMessage(ValidationErrors.DuplicatedExerciseIds);
    }
}
