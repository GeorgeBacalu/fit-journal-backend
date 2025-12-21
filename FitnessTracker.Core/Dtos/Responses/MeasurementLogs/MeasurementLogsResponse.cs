namespace FitnessTracker.Core.Dtos.Responses.MeasurementLogs;

public record MeasurementLogsResponse
{
    public IEnumerable<ShortMeasurementLogResponse> MeasurementLogs { get; init; } = [];
    public int TotalCount { get; init; }
}
