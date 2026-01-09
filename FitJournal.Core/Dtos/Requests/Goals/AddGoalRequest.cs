using FitJournal.Core.Constants;
using FitJournal.Domain.Enums.Goals;
using FluentValidation;

namespace FitJournal.Core.Dtos.Requests.Goals;

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
            .NotEmpty().WithMessage(ValidationErrors.Common.NameRequired.Message)
            .MaximumLength(50).WithMessage(ValidationErrors.Common.NameTooLong.Message);

        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage(ValidationErrors.Goals.InvalidDescriptionLength.Message);

        RuleFor(x => x.Notes)
            .MaximumLength(250).WithMessage(ValidationErrors.Goals.InvalidNotesLength.Message);

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage(ValidationErrors.Goals.TypeRequired.Message)
            .IsInEnum().WithMessage(ValidationErrors.Goals.InvalidType.Message);

        RuleFor(x => x.TargetWeight)
            .NotEmpty().WithMessage(ValidationErrors.Goals.TargetWeightRequired.Message)
            .InclusiveBetween(25, 250).WithMessage(ValidationErrors.Goals.InvalidTargetWeight.Message);

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage(ValidationErrors.Goals.StartDateRequired.Message)
            .Must((x, startDate) => startDate <= x.EndDate).WithMessage(ValidationErrors.Goals.StartBeforeEnd.Message);

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage(ValidationErrors.Goals.EndDateRequired.Message)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage(ValidationErrors.Goals.InvalidEndDate.Message);
    }
}
