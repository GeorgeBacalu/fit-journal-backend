using FitnessTracker.Core.Dtos.Requests.Goals;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Core.Interfaces.Repositories;

public interface IGoalRepository : IUserOwnedRepository<Goal>
{
    IQueryable<Goal> GetAllBaseQuery(GoalPaginationRequest request, Guid? userId);

    IQueryable<Goal> GetAllQuery(GoalPaginationRequest request, Guid? userId);
}
