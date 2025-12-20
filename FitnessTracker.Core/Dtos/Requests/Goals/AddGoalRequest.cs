using FitnessTracker.Domain.Enums;
using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.Goals;

public record AddGoalRequest
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public GoalType? Type { get; init; }
    public int? TargetWeight { get; init; }
    public DateOnly? StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
}

public class AddGoalValidator : AbstractValidator<AddGoalRequest>
{
    public AddGoalValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage(ValidationErrors.Goals.NameRequired)

            .MaximumLength(50)
            .WithMessage(ValidationErrors.Goals.InvalidNameLength);

        RuleFor(request => request.Description)
            .MaximumLength(250)
            .WithMessage(ValidationErrors.Goals.InvalidDescriptionLength);

        RuleFor(request => request.Type)
            .NotEmpty()
            .WithMessage(ValidationErrors.Goals.TypeRequired);

        RuleFor(request => request.TargetWeight)
            .NotEmpty()
            .WithMessage(ValidationErrors.Goals.TargetWeightRequired)

            .InclusiveBetween(25, 250)
            .WithMessage(ValidationErrors.Goals.InvalidTargetWeight);

        RuleFor(request => request.StartDate)
            .NotEmpty()
            .WithMessage(ValidationErrors.Goals.StartDateRequired)

            .Must((request, startDate) => startDate <= request.EndDate)
            .WithMessage(ValidationErrors.Goals.StartBeforeEnd);

        RuleFor(request => request.EndDate)
            .NotEmpty()
            .WithMessage(ValidationErrors.Goals.EndDateRequired)

            .Must(endDate => endDate >= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage(ValidationErrors.Goals.InvalidEndDate);
    }
}
