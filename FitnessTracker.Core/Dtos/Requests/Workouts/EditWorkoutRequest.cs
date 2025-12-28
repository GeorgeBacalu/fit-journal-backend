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
            .NotEmpty().WithMessage(ValidationErrors.Workouts.IdRequired.Message)
            .Must(id => id != Guid.Empty).WithMessage(ValidationErrors.Workouts.InvalidId.Message);

        Include(new AddWorkoutValidator());
    }
}
