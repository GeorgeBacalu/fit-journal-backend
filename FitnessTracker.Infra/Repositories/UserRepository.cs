using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Repositories;

public class UserRepository(FitnessTrackerContext context)
    : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken token = default) =>
        await context.Users.AsNoTracking().SingleOrDefaultAsync(user => user.Email == email, token);
}
