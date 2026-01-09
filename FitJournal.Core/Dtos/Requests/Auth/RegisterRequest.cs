using FitJournal.Core.Constants;
using FitJournal.Core.Dtos.Requests.Users;
using FitJournal.Domain.Enums.Users;
using FluentValidation;

namespace FitJournal.Core.Dtos.Requests.Auth;

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
            .NotEmpty().WithMessage(ValidationErrors.Common.NameRequired.Message)
            .MaximumLength(50).WithMessage(ValidationErrors.Common.NameTooLong.Message);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ValidationErrors.Users.EmailRequired.Message)
            .EmailAddress().WithMessage(ValidationErrors.Users.InvalidEmail.Message)
            .MaximumLength(50).WithMessage(ValidationErrors.Users.EmailTooLong.Message);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ValidationErrors.Users.PasswordRequired.Message)
            .Matches(ValidationRules.Users.PasswordRegex).WithMessage(ValidationErrors.Users.InvalidPassword.Message)
            .Length(6, 30).WithMessage(ValidationErrors.Users.InvalidPasswordLength.Message);

        RuleFor(x => x.ConfirmedPassword)
            .NotEmpty().WithMessage(ValidationErrors.Users.ConfirmPassword.Message)
            .Equal(x => x.Password).WithMessage(ValidationErrors.Users.PasswordsMismatch.Message);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage(ValidationErrors.Users.PhoneRequired.Message)
            .Matches(ValidationRules.Users.PhoneRegex).WithMessage(ValidationErrors.Users.InvalidPhone.Message)
            .MaximumLength(20).WithMessage(ValidationErrors.Users.PhoneTooLong.Message);

        RuleFor(x => x.Birthday)
            .NotEmpty().WithMessage(ValidationErrors.Users.BirthdayRequired.Message)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage(ValidationErrors.Users.FutureBirthday.Message)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-13)).WithMessage(ValidationErrors.Users.AgeRestriction.Message);

        RuleFor(x => x.Height)
            .NotEmpty().WithMessage(ValidationErrors.Users.HeightRequired.Message)
            .InclusiveBetween(120, 250).WithMessage(ValidationErrors.Users.HeightOutOfRange.Message);

        RuleFor(x => x.Weight)
            .NotEmpty().WithMessage(ValidationErrors.Users.WeightRequired.Message)
            .InclusiveBetween(25, 250).WithMessage(ValidationErrors.Users.WeightOutOfRange.Message);

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage(ValidationErrors.Users.GenderRequired.Message)
            .IsInEnum().WithMessage(ValidationErrors.Users.InvalidGender.Message);
    }
}
