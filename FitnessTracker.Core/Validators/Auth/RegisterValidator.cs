using FitnessTracker.Core.Dtos.Requests.Auth;
using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Validators.Auth;

public class RegisterValidator : AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage(ValidationErrors.NameRequired)

            .MaximumLength(50)
            .WithMessage(ValidationErrors.InvalidNameLength);

        RuleFor(request => request.Email)
            .NotEmpty()
            .WithMessage(ValidationErrors.EmailRequired)

            .EmailAddress()
            .WithMessage(ValidationErrors.InvalidEmail)

            .MaximumLength(50)
            .WithMessage(ValidationErrors.InvalidEmailLength);

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage(ValidationErrors.PasswordRequired)

            .Matches(ValidationRules.PasswordRegex)
            .WithMessage(ValidationErrors.InvalidPassword)

            .Length(6, 30)
            .WithMessage(ValidationErrors.InvalidPasswordLength);

        RuleFor(request => request.ConfirmedPassword)
            .NotEmpty()
            .WithMessage(ValidationErrors.ConfirmPassword)

            .Equal(request => request.Password)
            .WithMessage(ValidationErrors.PasswordsMismatch);

        RuleFor(request => request.Phone)
            .NotEmpty()
            .WithMessage(ValidationErrors.PhoneRequired)

            .Matches(ValidationRules.PhoneRegex)
            .WithMessage(ValidationErrors.InvalidPhone)

            .MaximumLength(20)
            .WithMessage(ValidationErrors.InvalidPhoneLength);

        RuleFor(request => request.Birthday)
            .NotEmpty()
            .WithMessage(ValidationErrors.BirthdayRequired)

            .Must(birthday => birthday <= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage(ValidationErrors.InvalidBirthday)

            .Must(birthday => birthday <= DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-13))
            .WithMessage(ValidationErrors.AgeRestriction);

        RuleFor(request => request.Height)
            .NotEmpty()
            .WithMessage(ValidationErrors.HeightRequired)

            .InclusiveBetween(120, 250)
            .WithMessage(ValidationErrors.InvalidHeight);

        RuleFor(request => request.Weight)
            .NotEmpty()
            .WithMessage(ValidationErrors.WeightRequired)

            .InclusiveBetween(25, 250)
            .WithMessage(ValidationErrors.InvalidWeight);

        RuleFor(request => request.Gender)
            .NotEmpty()
            .WithMessage(ValidationErrors.GenderRequired);
    }
}
