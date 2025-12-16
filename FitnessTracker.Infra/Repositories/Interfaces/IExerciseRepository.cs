using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Infra.Repositories.Interfaces;

public interface IExerciseRepository
{
    Task AddAsync(Exercise exercise, CancellationToken token = default);
}
