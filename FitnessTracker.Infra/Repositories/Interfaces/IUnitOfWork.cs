namespace FitnessTracker.Infra.Repositories.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IWorkoutRepository WorkoutRepository { get; }

    Task CommitAsync(CancellationToken token = default);
}
