using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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
    public virtual DbSet<FoodItem> FoodItems => Set<FoodItem>();
    public virtual DbSet<FoodLog> FoodLogs => Set<FoodLog>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(6, 2);
        configurationBuilder.Properties<Enum>().HaveConversion<string>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            if (entry.State == EntityState.Modified)
                entry.Entity.UpdatedAt = now;

        return base.SaveChangesAsync(cancellationToken);
    }
}
