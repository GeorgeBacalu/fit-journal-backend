using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.Infra.Repositories;

public class UnitOfWork(FitnessTrackerContext context,
    IUserRepository userRepository,
    IWorkoutRepository workoutRepository,
    IExerciseRepository exerciseRepository) : IUnitOfWork
{
    public IUserRepository UserRepository => userRepository;
    public IWorkoutRepository WorkoutRepository => workoutRepository;
    public IExerciseRepository ExerciseRepository => exerciseRepository;

    public Task CommitAsync(CancellationToken token = default)
        => context.SaveChangesAsync(token);
}
