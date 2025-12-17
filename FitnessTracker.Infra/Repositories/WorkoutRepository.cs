using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Repositories;

public class WorkoutRepository(FitnessTrackerContext context)
    : BaseRepository<Workout>(context), IWorkoutRepository
{
    public async Task<IEnumerable<Workout>> GetAllByIdsAsync(IEnumerable<Guid> ids, CancellationToken token = default) =>
        await context.Workouts
            .Where(workout => ids.Contains(workout.Id))
            .ToListAsync(token);
}
