using FitnessTracker.Core.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.ProgressLogs;

public record EditProgressLogRequest : AddProgressLogRequest
{
    public Guid Id { get; init; }
}

public class EditProgressLogValidator : AbstractValidator<EditProgressLogRequest>
{
    public EditProgressLogValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ValidationErrors.ProgressLogs.IdRequired)
            .Must(id => id != Guid.Empty).WithMessage(ValidationErrors.ProgressLogs.InvalidId);

        Include(new AddProgressLogValidator());
    }
}
