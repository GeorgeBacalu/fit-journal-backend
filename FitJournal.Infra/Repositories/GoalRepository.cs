using FitJournal.Core.Dtos.Requests.Goals;
using FitJournal.Core.Extensions.Pagination;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Domain.Entities;
using FitJournal.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FitJournal.Infra.Repositories;

public class GoalRepository(AppDbContext db)
    : UserOwnedRepository<Goal>(db), IGoalRepository
{
    public IQueryable<Goal> GetAllBaseQuery(GoalPaginationRequest request, Guid? userId) =>
        _db.Goals.AsNoTracking().Where(w => userId == null || w.UserId == userId).AddFilters(request);

    public IQueryable<Goal> GetAllQuery(GoalPaginationRequest request, Guid? userId) =>
        GetAllBaseQuery(request, userId).AddSorting(request).AddPaging(request);
}
