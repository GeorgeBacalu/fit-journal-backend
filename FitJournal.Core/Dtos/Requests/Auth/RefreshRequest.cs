using FitJournal.Core.Constants;
using FluentValidation;

namespace FitJournal.Core.Dtos.Requests.Auth;

public record RefreshRequest
{
    public required string RefreshToken { get; init; }
}

public class RefreshValidator : AbstractValidator<RefreshRequest>
{
    public RefreshValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage(ValidationErrors.Auth.RefreshTokenRequired.Message);
    }
}
