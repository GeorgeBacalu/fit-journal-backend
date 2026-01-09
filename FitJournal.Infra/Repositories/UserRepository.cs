using FitJournal.Core.Dtos.Requests.Users;
using FitJournal.Core.Extensions.Pagination;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Domain.Entities;
using FitJournal.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FitJournal.Infra.Repositories;

public class UserRepository(AppDbContext db)
    : BaseRepository<User>(db), IUserRepository
{
    public IQueryable<User> GetAllBaseQuery(UserPaginationRequest request) =>
        _db.Users.AsNoTracking().AddFilters(request);

    public IQueryable<User> GetAllQuery(UserPaginationRequest request) =>
        GetAllBaseQuery(request).AddSorting(request).AddPaging(request);
}
