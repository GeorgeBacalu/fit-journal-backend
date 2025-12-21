using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.MeasurementLogs;

public record RemoveMeasurementLogsRequest
{
    public IEnumerable<Guid> Ids { get; init; } = [];
    public bool HardDelete { get; init; }
}

public class RemoveMeasurementLogsValidator : AbstractValidator<RemoveMeasurementLogsRequest>
{
    public RemoveMeasurementLogsValidator()
    {
        RuleFor(request => request.Ids)
            .NotEmpty()
            .WithMessage(ValidationErrors.MeasurementLogs.IdsRequired)

            .Must(ids => ids.Distinct().Count() == ids.Count())
            .WithMessage(ValidationErrors.MeasurementLogs.DuplicatedIds);
    }
}
