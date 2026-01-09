using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Infra.Context;

namespace FitJournal.Infra.Repositories;

public class UnitOfWork(AppDbContext db) : IUnitOfWork
{
    private IUserRepository? users;
    private IWorkoutRepository? workouts;
    private IExerciseRepository? exercises;
    private IWorkoutExerciseRepository? workoutExercises;
    private IGoalRepository? goals;
    private IFoodItemRepository? foodItems;
    private IFoodLogRepository? foodLogs;
    private IProgressLogRepository? progressLogs;
    private IRequestLogRepository? requestLogs;

    public IUserRepository Users => users ??= new UserRepository(db);
    public IWorkoutRepository Workouts => workouts ??= new WorkoutRepository(db);
    public IExerciseRepository Exercises => exercises ??= new ExerciseRepository(db);
    public IWorkoutExerciseRepository WorkoutExercises => workoutExercises ??= new WorkoutExerciseRepository(db);
    public IGoalRepository Goals => goals ??= new GoalRepository(db);
    public IFoodItemRepository FoodItems => foodItems ??= new FoodItemRepository(db);
    public IFoodLogRepository FoodLogs => foodLogs ??= new FoodLogRepository(db);
    public IProgressLogRepository ProgressLogs => progressLogs ??= new ProgressLogRepository(db);
    public IRequestLogRepository RequestLogs => requestLogs ??= new RequestLogRepository(db);

    public async Task<int> CommitAsync(CancellationToken token) =>
        await db.SaveChangesAsync(token);
}
