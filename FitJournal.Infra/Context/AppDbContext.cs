using FitJournal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FitJournal.Infra.Context;

public class AppDbContext : DbContext
{
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
    public virtual DbSet<OAuthExchangeCode> OAuthExchangeCodes => Set<OAuthExchangeCode>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(7, 2);
        configurationBuilder.Properties<Enum>().HaveConversion<string>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // The production model targets SQL Server, while integration tests use SQLite.
        // Keep equivalent constraints in both providers so EnsureCreated can build a
        // faithful test database instead of evaluating SQL Server-only functions.
        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            var decimalConverter = new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<decimal, double>(
                value => Convert.ToDouble(value),
                value => Convert.ToDecimal(value));
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                         .SelectMany(entity => entity.GetProperties())
                         .Where(property => property.ClrType == typeof(decimal)))
                property.SetValueConverter(decimalConverter);

            modelBuilder.Entity<User>().ToTable(t =>
                t.HasCheckConstraint(
                    "CK_Users_AgeRestriction",
                    "date([Birthday], '+13 years') <= CURRENT_TIMESTAMP"));
            modelBuilder.Entity<ResetToken>().ToTable(t =>
                t.HasCheckConstraint(
                    "CK_ResetTokens_Token_Length",
                    "length([Token]) BETWEEN 100 AND 512 AND [Token] LIKE '%.%.%'"));
            modelBuilder.Entity<RequestLog>().Property(x => x.ExceptionStackTrace).HasColumnType("TEXT");
            modelBuilder.Entity<RequestLog>().Property(x => x.InnerExceptionStackTrace).HasColumnType("TEXT");
            modelBuilder.Entity<RequestLog>().Property(x => x.RequestHeader).HasColumnType("TEXT");
            modelBuilder.Entity<RequestLog>().Property(x => x.ResponseHeader).HasColumnType("TEXT");
        }

        foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(entity => entity.GetForeignKeys()))
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
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
