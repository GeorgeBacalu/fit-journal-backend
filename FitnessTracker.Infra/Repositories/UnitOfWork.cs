using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.Infra.Repositories;

public class UnitOfWork(FitnessTrackerContext context) : IUnitOfWork
{
    private IUserRepository? _users;
    private IWorkoutRepository? _workouts;
    private IExerciseRepository? _exercises;
    private IGoalRepository? _goals;
    private IFoodItemRepository _foodItems;

    public IUserRepository Users => _users ??= new UserRepository(context);
    public IWorkoutRepository Workouts => _workouts ??= new WorkoutRepository(context);
    public IExerciseRepository Exercises => _exercises ??= new ExerciseRepository(context);
    public IGoalRepository Goals => _goals ??= new GoalRepository(context);
    public IFoodItemRepository FoodItems => _foodItems ??= new FoodItemRepository(context);

    public Task CommitAsync(CancellationToken token = default)
        => context.SaveChangesAsync(token);
}
