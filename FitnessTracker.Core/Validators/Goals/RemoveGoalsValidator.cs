using FitnessTracker.Core.Dtos.Requests.Goals;
using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Validators.Goals;

public class RemoveGoalsValidator: AbstractValidator<RemoveGoalsRequest>
{
    public RemoveGoalsValidator()
    {
        RuleFor(request => request.Ids)
            .NotEmpty()
            .WithMessage(ValidationErrors.GoalIdsRequired)

            .Must(ids => ids.Distinct().Count() == ids.Count())
            .WithMessage(ValidationErrors.DuplicatedGoalIds);
    }
}
