using FitnessTracker.Core.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.ProgressLogs;

public record RemoveProgressLogsRequest
{
    public IEnumerable<Guid> Ids { get; init; } = [];
    public bool HardDelete { get; init; }
}

public class RemoveProgressLogsValidator : AbstractValidator<RemoveProgressLogsRequest>
{
    public RemoveProgressLogsValidator()
    {
        RuleFor(x => x.Ids)
            .NotEmpty().WithMessage(ValidationErrors.ProgressLogs.IdsRequired.Message)
            .Must(ids => ids.Distinct().Count() == ids.Count()).WithMessage(ValidationErrors.ProgressLogs.DuplicatedIds.Message);
    }
}
