using FitJournal.Core.Constants;
using FluentValidation;

namespace FitJournal.Core.Dtos.Requests.Auth;

public record ForgotPasswordRequest
{
    public required string Email { get; init; }
}

public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ValidationErrors.Users.EmailRequired.Message)
            .EmailAddress().WithMessage(ValidationErrors.Users.InvalidEmail.Message)
            .MaximumLength(50).WithMessage(ValidationErrors.Users.EmailTooLong.Message);
    }
}
