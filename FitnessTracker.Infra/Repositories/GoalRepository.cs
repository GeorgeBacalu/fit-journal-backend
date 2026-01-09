using FitnessTracker.Core.Dtos.Requests.Goals;
using FitnessTracker.Core.Extensions.Pagination;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Repositories;

public class GoalRepository(AppDbContext db)
    : UserOwnedRepository<Goal>(db), IGoalRepository
{
    public IQueryable<Goal> GetAllBaseQuery(GoalPaginationRequest request, Guid? userId) =>
        _db.Goals.AsNoTracking().Where(w => userId == null || w.UserId == userId).AddFilters(request);

    public IQueryable<Goal> GetAllQuery(GoalPaginationRequest request, Guid? userId) =>
        GetAllBaseQuery(request, userId).AddSorting(request).AddPaging(request);
}
