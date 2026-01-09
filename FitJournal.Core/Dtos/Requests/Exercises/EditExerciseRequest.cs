using FitJournal.Core.Constants;
using FluentValidation;

namespace FitJournal.Core.Dtos.Requests.Exercises;

public record EditExerciseRequest : AddExerciseRequest
{
    public Guid Id { get; init; }
}

public class EditExerciseValidator : AbstractValidator<EditExerciseRequest>
{
    public EditExerciseValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ValidationErrors.Exercises.IdRequired.Message)
            .Must(id => id != Guid.Empty).WithMessage(ValidationErrors.Exercises.InvalidId.Message);

        Include(new AddExerciseValidator());
    }
}
