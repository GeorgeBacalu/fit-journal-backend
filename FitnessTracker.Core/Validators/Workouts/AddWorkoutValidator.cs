using FitnessTracker.Core.Dtos.Requests.Workouts;
using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Validators.Workouts;

public class AddWorkoutValidator : AbstractValidator<AddWorkoutRequest>
{
    public AddWorkoutValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage(ValidationErrors.NameRequired)

            .MaximumLength(50)
            .WithMessage(ValidationErrors.InvalidNameLength);

        RuleFor(request => request.Description)
            .MaximumLength(250)
            .WithMessage(ValidationErrors.InvalidDescriptionLength);

        RuleFor(request => request.Notes)
            .MaximumLength(250)
            .WithMessage(ValidationErrors.InvalidNotesLength);

        RuleFor(request => request.DurationMinutes)
            .NotEmpty()
            .WithMessage(ValidationErrors.DurationRequired)

            .InclusiveBetween(5, 300)
            .WithMessage(ValidationErrors.InvalidDuration);

        RuleFor(request => request.StartedAt)
            .NotEmpty()
            .WithMessage(ValidationErrors.StartDateRequired)

            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage(ValidationErrors.InvalidStartDate);
    }
}
