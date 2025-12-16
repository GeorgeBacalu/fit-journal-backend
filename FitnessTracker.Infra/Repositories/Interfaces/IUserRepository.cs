using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Infra.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken token = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken token = default);
    Task AddAsync(User user, CancellationToken token = default);
}
