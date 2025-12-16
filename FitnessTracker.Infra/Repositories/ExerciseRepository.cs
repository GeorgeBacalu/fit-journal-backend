using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Repositories;

public class ExerciseRepository(FitnessTrackerContext context) : IExerciseRepository
{
    public async Task<IEnumerable<Exercise>> GetAllAsync(CancellationToken token = default)
        => await context.Exercises.ToListAsync(token);

    public async Task<Exercise?> GetByIdAsync(Guid id, CancellationToken token = default)
        => await context.Exercises.SingleOrDefaultAsync(exercise => exercise.Id == id, token);

    public async Task AddAsync(Exercise exercise, CancellationToken token = default)
        => await context.Exercises.AddAsync(exercise, token);
}
