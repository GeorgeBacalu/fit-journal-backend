using FitnessTracker.Core.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.Workouts;

public record EditWorkoutRequest : AddWorkoutRequest
{
    public Guid Id { get; init; }
}

public class EditWorkoutValidator : AbstractValidator<EditWorkoutRequest>
{
    public EditWorkoutValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ValidationErrors.Workouts.IdRequired)
            .Must(id => id != Guid.Empty).WithMessage(ValidationErrors.Workouts.InvalidId);

        Include(new AddWorkoutValidator());
    }
}
