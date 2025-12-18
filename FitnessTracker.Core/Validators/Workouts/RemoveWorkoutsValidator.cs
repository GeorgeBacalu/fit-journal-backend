using FitnessTracker.Core.Dtos.Requests.Workouts;
using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Validators.Workouts;

public class RemoveWorkoutsValidator : AbstractValidator<RemoveWorkoutsRequest>
{
    public RemoveWorkoutsValidator()
    {
        RuleFor(request => request.Ids)
            .NotEmpty()
            .WithMessage(ValidationErrors.WorkoutIdsRequired);
    }
}
