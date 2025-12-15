using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Repositories;

public class UserRepository(FitnessTrackerContext context) : IUserRepository
{
    public async Task AddAsync(User user, CancellationToken token = default)
        => await context.Users.AddAsync(user, token);

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken token = default)
        => await context.Users.AsNoTracking().SingleOrDefaultAsync(user => user.Id == id, token);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken token = default)
        => await context.Users.AsNoTracking().SingleOrDefaultAsync(user => user.Email == email, token);
}
