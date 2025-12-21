using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.Workouts;

public record AddWorkoutRequest
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
    public int? DurationMinutes { get; init; }
    public DateTime StartedAt { get; init; }
}

public class AddWorkoutValidator : AbstractValidator<AddWorkoutRequest>
{
    public AddWorkoutValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage(ValidationErrors.Common.NameRequired)

            .MaximumLength(50)
            .WithMessage(ValidationErrors.Common.InvalidNameLength);

        RuleFor(request => request.Description)
            .MaximumLength(250)
            .WithMessage(ValidationErrors.Workouts.InvalidDescriptionLength);

        RuleFor(request => request.Notes)
            .MaximumLength(250)
            .WithMessage(ValidationErrors.Workouts.InvalidNotesLength);

        RuleFor(request => request.DurationMinutes)
            .NotEmpty()
            .WithMessage(ValidationErrors.Workouts.DurationRequired)

            .InclusiveBetween(5, 300)
            .WithMessage(ValidationErrors.Workouts.InvalidDuration);

        RuleFor(request => request.StartedAt)
            .NotEmpty()
            .WithMessage(ValidationErrors.Workouts.StartDateRequired)

            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage(ValidationErrors.Workouts.FutureStartDate);
    }
}
