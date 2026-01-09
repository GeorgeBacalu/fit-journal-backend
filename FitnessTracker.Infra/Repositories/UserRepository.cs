using FitnessTracker.Core.Dtos.Requests.Users;
using FitnessTracker.Core.Extensions.Pagination;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Repositories;

public class UserRepository(AppDbContext db)
    : BaseRepository<User>(db), IUserRepository
{
    public IQueryable<User> GetAllBaseQuery(UserPaginationRequest request) =>
        _db.Users.AsNoTracking().AddFilters(request);

    public IQueryable<User> GetAllQuery(UserPaginationRequest request) =>
        GetAllBaseQuery(request).AddSorting(request).AddPaging(request);
}
