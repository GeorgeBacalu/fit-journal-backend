using FitJournal.Core.Constants;
using FluentValidation;

namespace FitJournal.Core.Dtos.Requests.Workouts;

public record AddWorkoutRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
    public int DurationMinutes { get; init; }
    public DateTime StartedAt { get; init; }
}

public class AddWorkoutValidator : AbstractValidator<AddWorkoutRequest>
{
    public AddWorkoutValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationErrors.Common.NameRequired.Message)
            .MaximumLength(50).WithMessage(ValidationErrors.Common.NameTooLong.Message);

        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage(ValidationErrors.Workouts.InvalidDescriptionLength.Message);

        RuleFor(x => x.Notes)
            .MaximumLength(250).WithMessage(ValidationErrors.Workouts.InvalidNotesLength.Message);

        RuleFor(x => x.DurationMinutes)
            .NotEmpty().WithMessage(ValidationErrors.Workouts.DurationRequired.Message)
            .InclusiveBetween(5, 300).WithMessage(ValidationErrors.Workouts.DurationOutOfRange.Message);

        RuleFor(x => x.StartedAt)
            .NotEmpty().WithMessage(ValidationErrors.Workouts.StartDateRequired.Message);
    }
}
