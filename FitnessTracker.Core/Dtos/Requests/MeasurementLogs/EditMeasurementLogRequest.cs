using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.MeasurementLogs;

public record EditMeasurementLogRequest : AddMeasurementLogRequest
{
    public Guid Id { get; init; }
}

public class EditMeasurementLogValidator : AbstractValidator<EditMeasurementLogRequest>
{
    public EditMeasurementLogValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty()
            .WithMessage(ValidationErrors.MeasurementLogs.IdRequired)

            .Must(id => id != Guid.Empty)
            .WithMessage(ValidationErrors.MeasurementLogs.InvalidId);

        Include(new AddMeasurementLogValidator());
    }
}
