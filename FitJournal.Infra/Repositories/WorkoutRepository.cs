using FitJournal.Core.Dtos.Requests.Workouts;
using FitJournal.Core.Extensions.Pagination;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Domain.Entities;
using FitJournal.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FitJournal.Infra.Repositories;

public class WorkoutRepository(AppDbContext db)
    : UserOwnedRepository<Workout>(db), IWorkoutRepository
{
    public IQueryable<Workout> GetAllBaseQuery(WorkoutPaginationRequest request, Guid? userId) =>
        _db.Workouts.AsNoTracking().Where(w => userId == null || w.UserId == userId).AddFilters(request);

    public IQueryable<Workout> GetAllQuery(WorkoutPaginationRequest request, Guid? userId) =>
        GetAllBaseQuery(request, userId).AddSorting(request).AddPaging(request);

    public async Task<Workout?> GetByIdAsync(Guid id, Guid? userId, CancellationToken token) =>
        await _db.Workouts.AsNoTracking()
            .Include(w => w.WorkoutExercises)
            .FirstOrDefaultAsync(w => w.UserId == userId && w.Id == id, token);
}
