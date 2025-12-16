using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.Infra.Repositories;

public class ExerciseRepository(FitnessTrackerContext context) : IExerciseRepository
{
    public async Task AddAsync(Exercise exercise, CancellationToken token = default)
        => await context.Exercises.AddAsync(exercise, token);
}
