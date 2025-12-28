namespace FitnessTracker.Core.Dtos.Responses.ProgressLogs;

public record ShortProgressLogResponse
{
    public Guid Id { get; init; }
    public DateOnly Date { get; init; }
    public decimal Weight { get; init; }
    public decimal BodyFat { get; init; }
}
