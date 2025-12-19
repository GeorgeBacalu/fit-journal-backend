using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Repositories;

public class GoalRepository(FitnessTrackerContext context)
    : BaseRepository<Goal>(context), IGoalRepository
{
    public IQueryable<Goal> GetAllByUserQuery(Guid userId, bool isAchieved = false) =>
        context.Goals.AsNoTracking().Where(goal => goal.UserId == userId && goal.IsAchieved == isAchieved);
}
