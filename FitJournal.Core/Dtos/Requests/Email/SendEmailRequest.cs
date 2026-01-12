using FitJournal.Core.Constants;
using FluentValidation;

namespace FitJournal.Core.Dtos.Requests.Email;

public record SendEmailRequest
{
    public required string To { get; init; }
    public required string Subject { get; init; }
    public required string Body { get; init; }
}

public class SendEmailValidator : AbstractValidator<SendEmailRequest>
{
    public SendEmailValidator()
    {
        RuleFor(x => x.To)
            .NotEmpty().WithMessage(ValidationErrors.Auth.RecipientEmailRequired.Message)
            .EmailAddress().WithMessage(ValidationErrors.Auth.InvalidRecipientEmail.Message)
            .MaximumLength(50).WithMessage(ValidationErrors.Auth.RecipientEmailTooLong.Message);

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage(ValidationErrors.Auth.EmailSubjectRequired.Message)
            .MaximumLength(255).WithMessage(ValidationErrors.Auth.EmailSubjectTooLong.Message);

        RuleFor(x => x.Body)
            .NotEmpty().WithMessage(ValidationErrors.Auth.EmailBodyRequired.Message);
    }
}
