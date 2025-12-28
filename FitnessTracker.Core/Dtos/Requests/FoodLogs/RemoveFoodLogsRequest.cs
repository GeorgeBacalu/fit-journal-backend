using FitnessTracker.Core.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.FoodLogs;

public record RemoveFoodLogsRequest
{
    public IEnumerable<Guid> Ids { get; init; } = [];
    public bool HardDelete { get; init; }
}

public class RemoveFoodLogsValidator : AbstractValidator<RemoveFoodLogsRequest>
{
    public RemoveFoodLogsValidator()
    {
        RuleFor(x => x.Ids)
            .NotEmpty().WithMessage(ValidationErrors.FoodLogs.IdsRequired.Message)
            .Must(ids => ids.Distinct().Count() == ids.Count()).WithMessage(ValidationErrors.FoodLogs.DuplicatedIds.Message);
    }
}
