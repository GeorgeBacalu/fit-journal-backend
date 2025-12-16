using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Config.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Context;

public class FitnessTrackerContext : DbContext
{
    public FitnessTrackerContext() { }
    public FitnessTrackerContext(DbContextOptions<FitnessTrackerContext> options)
        : base(options) { }

    public virtual DbSet<User> Users => Set<User>();
    public virtual DbSet<Workout> Workouts => Set<Workout>();
    public virtual DbSet<Exercise> Exercises => Set<Exercise>();
    public virtual DbSet<WorkoutExercise> WorkoutExercises => Set<WorkoutExercise>();
    public virtual DbSet<Goal> Goals => Set<Goal>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfig());
        modelBuilder.ApplyConfiguration(new WorkoutConfig());
        modelBuilder.ApplyConfiguration(new ExerciseConfig());
        modelBuilder.ApplyConfiguration(new WorkoutExerciseConfig());
        modelBuilder.ApplyConfiguration(new GoalConfig());
    }

    public override Task<int> SaveChangesAsync(CancellationToken token = default)
    {
        var now = DateTime.UtcNow;

        foreach (var entity in ChangeTracker.Entries<BaseEntity>())
            if (entity.State == EntityState.Modified)
                entity.Entity.UpdatedAt = now;

        return base.SaveChangesAsync(token);
    }
}
