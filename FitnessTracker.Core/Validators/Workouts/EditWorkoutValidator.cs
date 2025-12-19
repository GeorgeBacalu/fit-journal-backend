using FitnessTracker.Core.Dtos.Requests.Workouts;
using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Validators.Workouts;

public class EditWorkoutValidator : AbstractValidator<EditWorkoutRequest>
{
    public EditWorkoutValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty()
            .WithMessage(ValidationErrors.WorkoutIdRequired)

            .Must(id => id != Guid.Empty)
            .WithMessage(ValidationErrors.InvalidWorkoutId);

        Include(new AddWorkoutValidator());
    }
}
