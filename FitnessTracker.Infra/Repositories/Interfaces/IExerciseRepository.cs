using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Infra.Repositories.Interfaces;

public interface IExerciseRepository
{
    Task<IEnumerable<Exercise>> GetAllAsync(CancellationToken token = default);
    Task AddAsync(Exercise exercise, CancellationToken token = default);
}
