using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.Infra.Repositories;

public class UnitOfWork(FitnessTrackerContext context) : IUnitOfWork
{
    public IUserRepository? _users;
    public IWorkoutRepository? _workouts;
    public IExerciseRepository? _exercises;
    public IGoalRepository? _goals;

    public IUserRepository Users => _users ??= new UserRepository(context);
    public IWorkoutRepository Workouts => _workouts ??= new WorkoutRepository(context);
    public IExerciseRepository Exercises => _exercises ??= new ExerciseRepository(context);
    public IGoalRepository Goals => _goals ??= new GoalRepository(context);

    public Task CommitAsync(CancellationToken token = default)
        => context.SaveChangesAsync(token);
}
