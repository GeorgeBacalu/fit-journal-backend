using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.Infra.Repositories;

public class UnitOfWork(FitnessTrackerContext context) : IUnitOfWork
{
    public IUserRepository? _users;
    public IWorkoutRepository? _workouts;
    public IExerciseRepository? _exercises;
    public IGoalRepository? _goals;

    public IUserRepository UserRepository => _users ??= new UserRepository(context);
    public IWorkoutRepository WorkoutRepository => _workouts ??= new WorkoutRepository(context);
    public IExerciseRepository ExerciseRepository => _exercises ??= new ExerciseRepository(context);
    public IGoalRepository GoalRepository => _goals ??= new GoalRepository(context);

    public Task CommitAsync(CancellationToken token = default)
        => context.SaveChangesAsync(token);
}
