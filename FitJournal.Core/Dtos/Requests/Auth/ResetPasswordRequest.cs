using FitJournal.Core.Constants;
using FluentValidation;

namespace FitJournal.Core.Dtos.Requests.Auth;

public record ResetPasswordRequest
{
    public required string Token { get; init; }
    public required string NewPassword { get; init; }
    public required string ConfirmedPassword { get; init; }
}

public class ResetPasswordValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage(ValidationErrors.Auth.ResetTokenRequired.Message);

        RuleFor(X => X.NewPassword)
            .NotEmpty().WithMessage(ValidationErrors.Auth.NewPasswordRequired.Message)
            .Matches(ValidationRules.Users.PasswordRegex).WithMessage(ValidationErrors.Users.InvalidPassword.Message)
            .Length(6, 30).WithMessage(ValidationErrors.Users.InvalidPasswordLength.Message);

        RuleFor(x => x.ConfirmedPassword)
            .NotEmpty().WithMessage(ValidationErrors.Auth.ConfirmPassword.Message)
            .Equal(x => x.NewPassword).WithMessage(ValidationErrors.Users.PasswordsMismatch.Message);
    }
}
