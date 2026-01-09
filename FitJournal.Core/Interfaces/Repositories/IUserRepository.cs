using FitJournal.Core.Dtos.Requests.Users;
using FitJournal.Domain.Entities;

namespace FitJournal.Core.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    IQueryable<User> GetAllBaseQuery(UserPaginationRequest request);

    IQueryable<User> GetAllQuery(UserPaginationRequest request);
}
