namespace FitnessTracker.Core.Dtos.Responses.ProgressLogs;

public record ProgressLogResponse : ShortProgressLogResponse
{
    public decimal WaistCm { get; init; }
    public decimal ChestCm { get; init; }
    public decimal ArmsCm { get; init; }
}
