using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.Exercises;

public record EditExerciseRequest : AddExerciseRequest
{
    public Guid Id { get; init; }
}

public class EditExerciseValidator : AbstractValidator<EditExerciseRequest>
{
    public EditExerciseValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty()
            .WithMessage(ValidationErrors.Exercises.IdRequired)

            .Must(id => id != default)
            .WithMessage(ValidationErrors.Exercises.InvalidId);

        Include(new AddExerciseValidator());
    }
}
