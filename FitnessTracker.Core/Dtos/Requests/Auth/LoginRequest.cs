using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.Auth;

public record LoginRequest
{
    public string? Email { get; init; }
    public string? Password { get; init; }
}

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty()
            .WithMessage(ValidationErrors.Users.EmailRequired)

            .EmailAddress()
            .WithMessage(ValidationErrors.Users.InvalidEmail)

            .MaximumLength(50)
            .WithMessage(ValidationErrors.Users.InvalidEmailLength);

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage(ValidationErrors.Users.PasswordRequired);
    }
}
