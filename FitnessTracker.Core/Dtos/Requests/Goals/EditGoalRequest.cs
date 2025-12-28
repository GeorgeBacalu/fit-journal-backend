using FitnessTracker.Core.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.Goals;

public record EditGoalRequest : AddGoalRequest
{
    public Guid Id { get; init; }
}

public class EditGoalValidator : AbstractValidator<EditGoalRequest>
{
    public EditGoalValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ValidationErrors.Goals.IdRequired.Message)
            .Must(id => id != Guid.Empty).WithMessage(ValidationErrors.Goals.InvalidId.Message);

        Include(new AddGoalValidator());
    }
}
