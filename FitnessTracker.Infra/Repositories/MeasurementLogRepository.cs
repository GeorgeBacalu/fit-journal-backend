using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.Infra.Repositories;

public class MeasurementLogRepository(FitnessTrackerContext context)
    : UserOwnedRepository<MeasurementLog>(context), IMeasurementLogRepository
{
}
