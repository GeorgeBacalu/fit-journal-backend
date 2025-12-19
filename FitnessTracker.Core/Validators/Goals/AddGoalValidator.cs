using FitnessTracker.Core.Dtos.Requests.Goals;
using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Validators.Goals;

public class AddGoalValidator : AbstractValidator<AddGoalRequest>
{
    public AddGoalValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage(ValidationErrors.NameRequired)

            .MaximumLength(50)
            .WithMessage(ValidationErrors.InvalidNameLength);

        RuleFor(request => request.Description)
            .MaximumLength(250)
            .WithMessage(ValidationErrors.InvalidDescriptionLength);

        RuleFor(request => request.Type)
            .NotEmpty()
            .WithMessage(ValidationErrors.GoalTypeRequired);

        RuleFor(request => request.TargetWeight)
            .NotEmpty()
            .WithMessage(ValidationErrors.TargetWeightRequired)

            .InclusiveBetween(25, 250)
            .WithMessage(ValidationErrors.InvalidTargetWeight);

        RuleFor(request => request.StartDate)
            .NotEmpty()
            .WithMessage(ValidationErrors.StartDateRequired)

            .Must((request, startDate) => startDate <= request.EndDate)
            .WithMessage(ValidationErrors.StartBeforeEnd);

        RuleFor(request => request.EndDate)
            .NotEmpty()
            .WithMessage(ValidationErrors.EndDateRequired)

            .Must(endDate => endDate >= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage(ValidationErrors.InvalidEndDate);
    }
}
