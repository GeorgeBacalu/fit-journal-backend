namespace FitnessTracker.Infra.Repositories.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IWorkoutRepository WorkoutRepository { get; }
    IExerciseRepository ExerciseRepository { get; }
    IGoalRepository GoalRepository { get; }

    Task CommitAsync(CancellationToken token = default);
}
