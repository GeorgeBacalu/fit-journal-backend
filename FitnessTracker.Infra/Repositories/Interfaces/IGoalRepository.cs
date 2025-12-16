using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Infra.Repositories.Interfaces;

public interface IGoalRepository
{
    Task AddAsync(Goal goal, CancellationToken token = default);
}
