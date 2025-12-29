using FitnessTracker.Core.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.Auth;

public record LoginRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ValidationErrors.Users.EmailRequired.Message)
            .EmailAddress().WithMessage(ValidationErrors.Users.InvalidEmail.Message)
            .MaximumLength(50).WithMessage(ValidationErrors.Users.EmailTooLong.Message);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ValidationErrors.Users.PasswordRequired.Message);
    }
}
