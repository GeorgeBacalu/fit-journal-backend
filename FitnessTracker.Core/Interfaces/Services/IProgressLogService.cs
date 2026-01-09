using FitnessTracker.Core.Dtos.Requests.ProgressLogs;
using FitnessTracker.Core.Dtos.Responses.ProgressLogs;

namespace FitnessTracker.Core.Interfaces.Services;

public interface IProgressLogService : IBusinessService
{
    Task<IProgressLogsResponse> GetAllAsync(ProgressLogPaginationRequest request, Guid? userId, CancellationToken token);

    Task<ProgressLogResponse> GetByIdAsync(Guid id, Guid? userId, CancellationToken token);

    Task AddAsync(AddProgressLogRequest request, Guid userId, CancellationToken token);

    Task EditAsyc(EditProgressLogRequest request, Guid userId, CancellationToken token);

    Task RemoveRangeAsync(RemoveProgressLogsRequest request, Guid userId, CancellationToken token);
}
