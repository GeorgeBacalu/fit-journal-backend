using FitnessTracker.Core.Dtos.Requests.Users;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Core.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    IQueryable<User> GetAllBaseQuery(UserPaginationRequest request);

    IQueryable<User> GetAllQuery(UserPaginationRequest request);
}
