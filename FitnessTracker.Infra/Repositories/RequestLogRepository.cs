using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;

namespace FitnessTracker.Infra.Repositories;

public class RequestLogRepository(AppDbContext db)
    : BaseRepository<RequestLog>(db), IRequestLogRepository
{
}
