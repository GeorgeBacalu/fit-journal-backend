using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.Infra.Repositories;

public class GoalRepository(FitnessTrackerContext context) : IGoalRepository
{
    public async Task AddAsync(Goal goal, CancellationToken token = default)
        => await context.Goals.AddAsync(goal, token);
}
