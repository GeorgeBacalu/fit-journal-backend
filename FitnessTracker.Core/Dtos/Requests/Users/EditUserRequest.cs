using FitnessTracker.Domain.Enums;
using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.Users;

public record EditUserRequest
{
    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public DateOnly? Birthday { get; init; }
    public decimal? Height { get; init; }
    public decimal? Weight { get; init; }
    public Gender? Gender { get; init; }
}

public class EditUserValidator : AbstractValidator<EditUserRequest>
{
    public EditUserValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage(ValidationErrors.Common.NameRequired)

            .MaximumLength(50)
            .WithMessage(ValidationErrors.Common.InvalidNameLength);

        RuleFor(request => request.Email)
            .NotEmpty()
            .WithMessage(ValidationErrors.Users.EmailRequired)

            .EmailAddress()
            .WithMessage(ValidationErrors.Users.InvalidEmail)

            .MaximumLength(50)
            .WithMessage(ValidationErrors.Users.InvalidEmailLength);

        RuleFor(request => request.Phone)
            .NotEmpty()
            .WithMessage(ValidationErrors.Users.PhoneRequired)

            .Matches(ValidationRules.Users.PhoneRegex)
            .WithMessage(ValidationErrors.Users.InvalidPhone)

            .MaximumLength(20)
            .WithMessage(ValidationErrors.Users.InvalidPhoneLength);

        RuleFor(request => request.Birthday)
            .NotEmpty()
            .WithMessage(ValidationErrors.Users.BirthdayRequired)

            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage(ValidationErrors.Users.FutureBirthday)

            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-13))
            .WithMessage(ValidationErrors.Users.AgeRestriction);

        RuleFor(request => request.Height)
            .NotEmpty()
            .WithMessage(ValidationErrors.Users.HeightRequired)

            .InclusiveBetween(120, 250)
            .WithMessage(ValidationErrors.Users.InvalidHeight);

        RuleFor(request => request.Weight)
            .NotEmpty()
            .WithMessage(ValidationErrors.Users.WeightRequired)

            .InclusiveBetween(25, 250)
            .WithMessage(ValidationErrors.Users.InvalidWeight);

        RuleFor(request => request.Gender)
            .NotEmpty()
            .WithMessage(ValidationErrors.Users.GenderRequired);
    }
}
