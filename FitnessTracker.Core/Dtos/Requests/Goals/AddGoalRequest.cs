using FitnessTracker.Core.Constants;
using FitnessTracker.Domain.Enums;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.Goals;

public record AddGoalRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
    public GoalType Type { get; init; }
    public int TargetWeight { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
}

public class AddGoalValidator : AbstractValidator<AddGoalRequest>
{
    public AddGoalValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationErrors.Common.NameRequired)
            .MaximumLength(50).WithMessage(ValidationErrors.Common.NameTooLong);

        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage(ValidationErrors.Goals.InvalidDescriptionLength);

        RuleFor(x => x.Notes)
            .MaximumLength(250).WithMessage(ValidationErrors.Goals.InvalidNotesLength);

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage(ValidationErrors.Goals.TypeRequired)
            .IsInEnum().WithMessage(ValidationErrors.Goals.InvalidType);

        RuleFor(x => x.TargetWeight)
            .NotEmpty().WithMessage(ValidationErrors.Goals.TargetWeightRequired)
            .InclusiveBetween(25, 250).WithMessage(ValidationErrors.Goals.InvalidTargetWeight);

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage(ValidationErrors.Goals.StartDateRequired)
            .Must((x, startDate) => startDate <= x.EndDate).WithMessage(ValidationErrors.Goals.StartBeforeEnd);

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage(ValidationErrors.Goals.EndDateRequired)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage(ValidationErrors.Goals.InvalidEndDate);
    }
}
