using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Infra.Repositories.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken token = default);
}
