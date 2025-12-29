using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Repositories;

public class WorkoutRepository(AppDbContext db)
    : UserOwnedRepository<Workout>(db), IWorkoutRepository
{
    public new async Task<Workout?> GetByIdAsync(Guid id, Guid userId, CancellationToken token) =>
        await _db.Workouts.AsNoTracking()
            .Include(w => w.WorkoutExercises)
            .FirstOrDefaultAsync(w => w.UserId == userId && w.Id == id, token);
}
