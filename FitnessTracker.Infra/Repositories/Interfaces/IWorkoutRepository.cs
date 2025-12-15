using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Infra.Repositories.Interfaces;

public interface IWorkoutRepository
{
    Task<IEnumerable<Workout>> GetAllAsync(CancellationToken token = default);
    Task AddAsync(Workout workout, CancellationToken token = default);
}
