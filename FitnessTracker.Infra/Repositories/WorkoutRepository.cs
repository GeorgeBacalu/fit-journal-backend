using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Repositories;

public class WorkoutRepository(FitnessTrackerContext context) : IWorkoutRepository
{
    public async Task<IEnumerable<Workout>> GetAllAsync(CancellationToken token = default)
        => await context.Workouts.AsNoTracking().ToListAsync(token);

    public async Task<IEnumerable<Workout>> GetAllByUserIdAsync(Guid userId, CancellationToken token = default)
        => await context.Workouts.AsNoTracking()
            .Where(workout => workout.UserId == userId)
            .ToListAsync(token);

    public async Task<IEnumerable<Workout>> GetAllByIdsAsync(IEnumerable<Guid> ids, CancellationToken token = default)
        => await context.Workouts
            .Where(workout => ids.Contains(workout.Id))
            .ToListAsync(token);

    public async Task<Workout?> GetByIdAsync(Guid id, CancellationToken token = default)
        => await context.Workouts.SingleOrDefaultAsync(workout => workout.Id == id, token);

    public async Task AddAsync(Workout workout, CancellationToken token = default)
        => await context.Workouts.AddAsync(workout, token);
}
