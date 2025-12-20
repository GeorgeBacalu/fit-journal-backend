using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Repositories;

public class ExerciseRepository(FitnessTrackerContext context)
    : BaseRepository<Exercise>(context), IExerciseRepository
{
    public async Task<bool> AnyInUseAsync(IEnumerable<Guid> ids, CancellationToken token) =>
        await context.Exercises.AnyAsync(exercise =>
            ids.Contains(exercise.Id) && exercise.WorkoutExercises.Any(), token);
}
