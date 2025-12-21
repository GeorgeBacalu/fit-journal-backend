using FitnessTracker.Core.Dtos.Requests.MeasurementLogs;

namespace FitnessTracker.Core.Services.Interfaces;

public interface IMeasurementLogService
{
    Task AddAsync(AddMeasurementLogRequest request, Guid userId, CancellationToken token);
}
