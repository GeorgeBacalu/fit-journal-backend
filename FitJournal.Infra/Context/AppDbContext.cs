using FitJournal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FitJournal.Infra.Context;

public class AppDbContext : DbContext
{
    private const string SqliteProvider = "Microsoft.EntityFrameworkCore.Sqlite";

    public AppDbContext() { }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public virtual DbSet<User> Users => Set<User>();
    public virtual DbSet<Workout> Workouts => Set<Workout>();
    public virtual DbSet<Exercise> Exercises => Set<Exercise>();
    public virtual DbSet<WorkoutExercise> WorkoutExercises => Set<WorkoutExercise>();
    public virtual DbSet<Goal> Goals => Set<Goal>();
    public virtual DbSet<FoodItem> FoodItems => Set<FoodItem>();
    public virtual DbSet<FoodLog> FoodLogs => Set<FoodLog>();
    public virtual DbSet<ProgressLog> ProgressLogs => Set<ProgressLog>();
    public virtual DbSet<RequestLog> RequestLogs => Set<RequestLog>();
    public virtual DbSet<ResetToken> ResetTokens => Set<ResetToken>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(7, 2);
        configurationBuilder.Properties<Enum>().HaveConversion<string>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        if (Database.ProviderName == SqliteProvider)
            ConfigureSqliteModel(modelBuilder);

        foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(entity => entity.GetForeignKeys()))
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
    }

    private static void ConfigureSqliteModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RequestLog>(entity =>
        {
            entity.Property(rl => rl.ExceptionStackTrace).HasColumnType("TEXT");
            entity.Property(rl => rl.InnerExceptionStackTrace).HasColumnType("TEXT");
            entity.Property(rl => rl.RequestHeader).HasColumnType("TEXT");
            entity.Property(rl => rl.ResponseHeader).HasColumnType("TEXT");
        });

        modelBuilder.Entity<User>().ToTable(t =>
            t.HasCheckConstraint("CK_Users_AgeRestriction", "1 = 1"));

        modelBuilder.Entity<ResetToken>().ToTable(t =>
            t.HasCheckConstraint("CK_ResetTokens_Token_Length", "length([Token]) BETWEEN 100 AND 512 AND [Token] LIKE '%.%.%'"));
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            if (entry.State == EntityState.Modified)
                entry.Entity.UpdatedAt = now;

        return base.SaveChangesAsync(cancellationToken);
    }
}
