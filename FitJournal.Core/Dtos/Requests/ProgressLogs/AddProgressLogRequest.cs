using FitJournal.Core.Constants;
using FluentValidation;

namespace FitJournal.Core.Dtos.Requests.ProgressLogs;

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
            .NotEmpty().WithMessage(ValidationErrors.ProgressLogs.DateRequired.Message)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage(ValidationErrors.ProgressLogs.FutureDate.Message);

        RuleFor(x => x.Weight)
            .NotEmpty().WithMessage(ValidationErrors.ProgressLogs.WeightRequired.Message)
            .InclusiveBetween(25, 250).WithMessage(ValidationErrors.ProgressLogs.WeightOutOfRange.Message);

        RuleFor(x => x.BodyFat)
            .NotEmpty().WithMessage(ValidationErrors.ProgressLogs.BodyFatRequired.Message)
            .InclusiveBetween(2, 60).WithMessage(ValidationErrors.ProgressLogs.BodyFatOutOfRange.Message);

        RuleFor(x => x.WaistCm)
            .NotEmpty().WithMessage(ValidationErrors.ProgressLogs.WaistCmRequired.Message)
            .InclusiveBetween(30, 250).WithMessage(ValidationErrors.ProgressLogs.WaistCmOutOfRange.Message);

        RuleFor(x => x.ChestCm)
            .NotEmpty().WithMessage(ValidationErrors.ProgressLogs.ChestCmRequired.Message)
            .InclusiveBetween(30, 200).WithMessage(ValidationErrors.ProgressLogs.ChestCmOutOfRange.Message);

        RuleFor(x => x.ArmsCm)
            .NotEmpty().WithMessage(ValidationErrors.ProgressLogs.ArmsCmRequired.Message)
            .InclusiveBetween(10, 100).WithMessage(ValidationErrors.ProgressLogs.ArmsCmOutOfRange.Message);
    }
}
