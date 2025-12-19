using FitnessTracker.Core.Dtos.Requests.Goals;
using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Validators.Goals;

public class EditGoalValidator : AbstractValidator<EditGoalRequest>
{
    public EditGoalValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty()
            .WithMessage(ValidationErrors.GoalIdRequired)

            .Must(id => id != Guid.Empty)
            .WithMessage(ValidationErrors.InvalidGoalId);

        Include(new AddGoalValidator());
    }
}
