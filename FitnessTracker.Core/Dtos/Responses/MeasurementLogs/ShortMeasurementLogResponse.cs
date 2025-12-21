namespace FitnessTracker.Core.Dtos.Responses.MeasurementLogs;

public record ShortMeasurementLogResponse
{
    public DateOnly Date { get; init; }
    public decimal Weight { get; init; }
    public decimal BodyFatPercentage { get; init; }
    public decimal WaistCircumference { get; init; }
    public decimal ChestCircumference { get; init; }
    public decimal ArmsCircumference { get; init; }
}
