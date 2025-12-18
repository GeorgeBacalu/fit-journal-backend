using FitnessTracker.Core.Dtos.Requests.Exercises;
using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Validators.Exercises;

public class AddExerciseValidator : AbstractValidator<AddExerciseRequest>
{
    public AddExerciseValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage(ValidationErrors.NameRequired)

            .MaximumLength(50)
            .WithMessage(ValidationErrors.InvalidNameLength);

        RuleFor(request => request.Description)
            .MaximumLength(250)
            .WithMessage(ValidationErrors.InvalidDescriptionLength);

        RuleFor(request => request.Notes)
            .MaximumLength(250)
            .WithMessage(ValidationErrors.InvalidNotesLength);

        RuleFor(request => request.MuscleGroup)
            .NotEmpty()
            .WithMessage(ValidationErrors.MuscleGroupRequired);

        RuleFor(request => request.DifficultyLevel)
            .NotEmpty()
            .WithMessage(ValidationErrors.DifficultyLevelRequired);
    }
}
