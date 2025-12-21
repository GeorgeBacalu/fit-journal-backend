using FitnessTracker.Infra.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.MeasurementLogs;

public record AddMeasurementLogRequest
{
    public DateOnly? Date { get; init; }
    public decimal? Weight { get; init; }
    public decimal? BodyFatPercentage { get; init; }
    public decimal? WaistCircumference { get; init; }
    public decimal? ChestCircumference { get; init; }
    public decimal? ArmsCircumference { get; init; }
}

public class AddMeasurementLogValidator : AbstractValidator<AddMeasurementLogRequest>
{
    public AddMeasurementLogValidator()
    {
        RuleFor(request => request.Date)
            .NotEmpty()
            .WithMessage(ValidationErrors.MeasurementLogs.DateRequired)

            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage(ValidationErrors.MeasurementLogs.FutureDate);

        RuleFor(request => request.Weight)
            .NotEmpty()
            .WithMessage(ValidationErrors.MeasurementLogs.WeightRequired)

            .InclusiveBetween(25, 250)
            .WithMessage(ValidationErrors.MeasurementLogs.WeightOutOfRange);

        RuleFor(request => request.BodyFatPercentage)
            .NotEmpty()
            .WithMessage(ValidationErrors.MeasurementLogs.BodyFatPercentageRequired)

            .InclusiveBetween(2, 60)
            .WithMessage(ValidationErrors.MeasurementLogs.BodyFatPercentageOutOfRange);

        RuleFor(request => request.WaistCircumference)
            .NotEmpty()
            .WithMessage(ValidationErrors.MeasurementLogs.WaistCircumferenceRequired)

            .InclusiveBetween(30, 250)
            .WithMessage(ValidationErrors.MeasurementLogs.WaistCircumferenceOutOfRange);

        RuleFor(request => request.ChestCircumference)
            .NotEmpty()
            .WithMessage(ValidationErrors.MeasurementLogs.ChestCircumferenceRequired)

            .InclusiveBetween(30, 200)
            .WithMessage(ValidationErrors.MeasurementLogs.ChestCircumferenceOutOfRange);

        RuleFor(request => request.ArmsCircumference)
            .NotEmpty()
            .WithMessage(ValidationErrors.MeasurementLogs.ArmsCircumferenceRequired)

            .InclusiveBetween(10, 100)
            .WithMessage(ValidationErrors.MeasurementLogs.ArmsCircumferenceOutOfRange);
    }
}