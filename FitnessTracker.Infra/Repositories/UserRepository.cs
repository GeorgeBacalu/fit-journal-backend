using FitnessTracker.Infra.Context;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.Infra.Repositories;
public class UserRepository(FitnessTrackerContext context) : IUserRepository
{
    public async Task AddAsync(User user, CancellationToken token = default) => await context.AddAsync(user, token);
}
