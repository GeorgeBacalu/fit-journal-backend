using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Repositories;

public class ExerciseRepository(FitnessTrackerContext context)
    : BaseRepository<Exercise>(context), IExerciseRepository
{
    public async Task<IEnumerable<Exercise>> GetAllByIdsAsync(IEnumerable<Guid> ids, CancellationToken token = default) =>
        await context.Exercises
            .Where(workout => ids.Contains(workout.Id))
            .ToListAsync(token);
}
