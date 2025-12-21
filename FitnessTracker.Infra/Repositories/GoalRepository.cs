using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.Infra.Repositories;

public class GoalRepository(FitnessTrackerContext context)
    : UserOwnedRepository<Goal>(context), IGoalRepository
{
}
