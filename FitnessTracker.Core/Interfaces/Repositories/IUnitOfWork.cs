namespace FitnessTracker.Core.Interfaces.Repositories;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IWorkoutRepository Workouts { get; }
    IExerciseRepository Exercises { get; }
    IWorkoutExerciseRepository WorkoutExercises { get; }
    IGoalRepository Goals { get; }
    IFoodItemRepository FoodItems { get; }
    IFoodLogRepository FoodLogs { get; }
    IProgressLogRepository ProgressLogs { get; }

    Task<int> CommitAsync(CancellationToken token);
}
