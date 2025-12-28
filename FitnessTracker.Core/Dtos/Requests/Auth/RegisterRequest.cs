using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.Users;
using FitnessTracker.Domain.Enums;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.Auth;

public record RegisterRequest : IUserRequest
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string ConfirmedPassword { get; init; }
    public required string Phone { get; init; }
    public DateOnly Birthday { get; init; }
    public decimal Height { get; init; }
    public decimal Weight { get; init; }
    public Gender Gender { get; init; }
}

public class RegisterValidator : AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationErrors.Common.NameRequired)
            .MaximumLength(50).WithMessage(ValidationErrors.Common.NameTooLong);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ValidationErrors.Users.EmailRequired)
            .EmailAddress().WithMessage(ValidationErrors.Users.InvalidEmail)
            .MaximumLength(50).WithMessage(ValidationErrors.Users.EmailTooLong);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ValidationErrors.Users.PasswordRequired)
            .Matches(ValidationRules.Users.PasswordRegex).WithMessage(ValidationErrors.Users.InvalidPassword)
            .Length(6, 30).WithMessage(ValidationErrors.Users.InvalidPasswordLength);

        RuleFor(x => x.ConfirmedPassword)
            .NotEmpty().WithMessage(ValidationErrors.Users.ConfirmPassword)
            .Equal(x => x.Password).WithMessage(ValidationErrors.Users.PasswordsMismatch);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage(ValidationErrors.Users.PhoneRequired)
            .Matches(ValidationRules.Users.PhoneRegex).WithMessage(ValidationErrors.Users.InvalidPhone)
            .MaximumLength(20).WithMessage(ValidationErrors.Users.PhoneTooLong);

        RuleFor(x => x.Birthday)
            .NotEmpty().WithMessage(ValidationErrors.Users.BirthdayRequired)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage(ValidationErrors.Users.FutureBirthday)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-13)).WithMessage(ValidationErrors.Users.AgeRestriction);

        RuleFor(x => x.Height)
            .NotEmpty().WithMessage(ValidationErrors.Users.HeightRequired)
            .InclusiveBetween(120, 250).WithMessage(ValidationErrors.Users.HeightOutOfRange);

        RuleFor(x => x.Weight)
            .NotEmpty().WithMessage(ValidationErrors.Users.WeightRequired)
            .InclusiveBetween(25, 250).WithMessage(ValidationErrors.Users.WeightOutOfRange);

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage(ValidationErrors.Users.GenderRequired)
            .IsInEnum().WithMessage(ValidationErrors.Users.InvalidGender);
    }
}
