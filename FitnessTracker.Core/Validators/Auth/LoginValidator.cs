using FitnessTracker.Core.Dtos.Requests.Auth;
using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Validators.Auth;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty()
            .WithMessage(ValidationErrors.EmailRequired)

            .EmailAddress()
            .WithMessage(ValidationErrors.InvalidEmail)

            .MaximumLength(50)
            .WithMessage(ValidationErrors.InvalidEmailLength);

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage(ValidationErrors.PasswordRequired);
    }
}
