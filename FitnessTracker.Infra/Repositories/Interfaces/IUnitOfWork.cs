namespace FitnessTracker.Infra.Repositories.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IWorkoutRepository Workouts { get; }
    IExerciseRepository Exercises { get; }
    IGoalRepository Goals { get; }
    IFoodItemRepository FoodItems { get; }

    Task CommitAsync(CancellationToken token = default);
}
