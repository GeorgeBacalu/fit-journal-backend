using FitJournal.Core.Dtos.Requests.Goals;
using FitJournal.Domain.Entities;

namespace FitJournal.Core.Interfaces.Repositories;

public interface IGoalRepository : IUserOwnedRepository<Goal>
{
    IQueryable<Goal> GetAllBaseQuery(GoalPaginationRequest request, Guid? userId);

    IQueryable<Goal> GetAllQuery(GoalPaginationRequest request, Guid? userId);
}
