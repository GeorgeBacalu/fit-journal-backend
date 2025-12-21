using FitnessTracker.Core.Dtos.Requests.MeasurementLogs;
using FitnessTracker.Core.Dtos.Responses.MeasurementLogs;

namespace FitnessTracker.Core.Services.Interfaces;

public interface IMeasurementLogService
{
    Task<MeasurementLogsResponse> GetAllAsync(Guid userId, CancellationToken token);

    Task<MeasurementLogService> GetByIdAsync(Guid id, Guid userId, CancellationToken token);

    Task AddAsync(AddMeasurementLogRequest request, Guid userId, CancellationToken token);

    Task EditAsyc(EditMeasurementLogRequest request, Guid userId, CancellationToken token);
}
