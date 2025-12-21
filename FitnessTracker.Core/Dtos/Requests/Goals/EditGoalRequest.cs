using FitnessTracker.Infra.Constants;
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
        RuleFor(request => request.Id)
            .NotEmpty()
            .WithMessage(ValidationErrors.Goals.IdRequired)

            .Must(id => id != Guid.Empty)
            .WithMessage(ValidationErrors.Goals.InvalidId);

        Include(new AddGoalValidator());
    }
}
