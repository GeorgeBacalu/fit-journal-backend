using FitnessTracker.Core.Dtos.Requests.Exercises;
using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Validators.Exercises;

public class EditExerciseValidator : AbstractValidator<EditExerciseRequest>
{
    public EditExerciseValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty()
            .WithMessage(ValidationErrors.ExerciseIdRequired)

            .Must(id => id != Guid.Empty)
            .WithMessage(ValidationErrors.InvalidExerciseId);

        Include(new EditExerciseValidator());
    }
}
