using FitJournal.Core.Constants;
using FluentValidation;

namespace FitJournal.Core.Dtos.Requests.WorkoutExercises;

public record EditWorkoutExerciseRequest : AddWorkoutExerciseRequest
{
    public Guid Id { get; init; }
}

public class EditWorkoutExerciseValidator : AbstractValidator<EditWorkoutExerciseRequest>
{
    public EditWorkoutExerciseValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ValidationErrors.WorkoutExercises.IdRequired.Message)
            .Must(id => id != Guid.Empty).WithMessage(ValidationErrors.WorkoutExercises.InvalidId.Message);

        Include(new AddWorkoutExerciseValidator());
    }
}
