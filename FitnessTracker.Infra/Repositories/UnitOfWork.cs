using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.Infra.Repositories;

public class UnitOfWork(FitnessTrackerContext context) : IUnitOfWork
{
    private IUserRepository? users;
    private IWorkoutRepository? workouts;
    private IExerciseRepository? exercises;
    private IGoalRepository? goals;
    private IFoodItemRepository? foodItems;
    private IFoodLogRepository? foodLogs;
    private IMeasurementLogRepository? measurementLogs;

    public IUserRepository Users => users ??= new UserRepository(context);
    public IWorkoutRepository Workouts => workouts ??= new WorkoutRepository(context);
    public IExerciseRepository Exercises => exercises ??= new ExerciseRepository(context);
    public IGoalRepository Goals => goals ??= new GoalRepository(context);
    public IFoodItemRepository FoodItems => foodItems ??= new FoodItemRepository(context);
    public IFoodLogRepository FoodLogs => foodLogs ??= new FoodLogRepository(context);
    public IMeasurementLogRepository MeasurementLogs => measurementLogs ??= new MeasurementLogRepository(context);

    public Task<int> CommitAsync(CancellationToken token) =>
        context.SaveChangesAsync(token);
}
