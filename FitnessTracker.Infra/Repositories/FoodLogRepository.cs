using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.Infra.Repositories;

public class FoodLogRepository(FitnessTrackerContext context)
    : UserOwnedRepository<FoodLog>(context), IFoodLogRepository
{
}
