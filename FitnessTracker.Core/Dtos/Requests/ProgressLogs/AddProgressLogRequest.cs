using FitnessTracker.Core.Constants;
using FluentValidation;

namespace FitnessTracker.Core.Dtos.Requests.ProgressLogs;

public record AddProgressLogRequest
{
    public DateOnly Date { get; init; }
    public decimal Weight { get; init; }
    public decimal BodyFat { get; init; }
    public decimal WaistCm { get; init; }
    public decimal ChestCm { get; init; }
    public decimal ArmsCm { get; init; }
}

public class AddProgressLogValidator : AbstractValidator<AddProgressLogRequest>
{
    public AddProgressLogValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage(ValidationErrors.ProgressLogs.DateRequired)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage(ValidationErrors.ProgressLogs.FutureDate);

        RuleFor(x => x.Weight)
            .NotEmpty().WithMessage(ValidationErrors.ProgressLogs.WeightRequired)
            .InclusiveBetween(25, 250).WithMessage(ValidationErrors.ProgressLogs.WeightOutOfRange);

        RuleFor(x => x.BodyFat)
            .NotEmpty().WithMessage(ValidationErrors.ProgressLogs.BodyFatRequired)
            .InclusiveBetween(2, 60).WithMessage(ValidationErrors.ProgressLogs.BodyFatOutOfRange);

        RuleFor(x => x.WaistCm)
            .NotEmpty().WithMessage(ValidationErrors.ProgressLogs.WaistCmRequired)
            .InclusiveBetween(30, 250).WithMessage(ValidationErrors.ProgressLogs.WaistCmOutOfRange);

        RuleFor(x => x.ChestCm)
            .NotEmpty().WithMessage(ValidationErrors.ProgressLogs.ChestCmRequired)
            .InclusiveBetween(30, 200).WithMessage(ValidationErrors.ProgressLogs.ChestCmOutOfRange);

        RuleFor(x => x.ArmsCm)
            .NotEmpty().WithMessage(ValidationErrors.ProgressLogs.ArmsCmRequired)
            .InclusiveBetween(10, 100).WithMessage(ValidationErrors.ProgressLogs.ArmsCmOutOfRange);
    }
}
