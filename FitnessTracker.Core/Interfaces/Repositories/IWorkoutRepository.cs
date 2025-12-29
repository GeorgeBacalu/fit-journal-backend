using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Core.Interfaces.Repositories;

public interface IWorkoutRepository : IUserOwnedRepository<Workout>
{
    new Task<Workout?> GetByIdAsync(Guid id, Guid userId, CancellationToken token);
}
