using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Domain.Entities;
using FitJournal.Infra.Context;

namespace FitJournal.Infra.Repositories;

public class RequestLogRepository(AppDbContext db)
    : BaseRepository<RequestLog>(db), IRequestLogRepository
{
}
