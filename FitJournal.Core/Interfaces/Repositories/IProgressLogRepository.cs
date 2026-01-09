using FitJournal.Core.Dtos.Requests.ProgressLogs;
using FitJournal.Domain.Entities;

namespace FitJournal.Core.Interfaces.Repositories;

public interface IProgressLogRepository : IUserOwnedRepository<ProgressLog>
{
    IQueryable<ProgressLog> GetAllBaseQuery(ProgressLogPaginationRequest request, Guid? userId);

    IQueryable<ProgressLog> GetAllQuery(ProgressLogPaginationRequest request, Guid? userId);

    Task<ProgressLog?> GetLastAsync(Guid userId, CancellationToken token);
}
