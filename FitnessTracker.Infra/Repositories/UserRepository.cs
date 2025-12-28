using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;

namespace FitnessTracker.Infra.Repositories;

public class UserRepository(AppDbContext db)
    : BaseRepository<User>(db), IUserRepository
{
}
