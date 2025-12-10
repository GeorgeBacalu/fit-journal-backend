using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Context;

public class FitnessTrackerContext : DbContext
{
    public FitnessTrackerContext() { }
    public FitnessTrackerContext(DbContextOptions<FitnessTrackerContext> options) : base(options) { }

    public virtual DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(user => user.Name).IsUnique();
            entity.HasIndex(user => user.Email).IsUnique();

            entity.Property(user => user.Name).IsRequired().HasMaxLength(50);
            entity.Property(user => user.Email).IsRequired().HasMaxLength(50);
            entity.Property(user => user.PasswordHash).IsRequired().HasMaxLength(65);
            entity.Property(user => user.Phone).IsRequired().HasMaxLength(20);

            entity.Property(user => user.Gender).IsRequired().HasConversion<string>().HasMaxLength(8);
            entity.Property(user => user.Role).IsRequired().HasConversion<string>().HasMaxLength(6);

            entity.Property(user => user.UpdatedAt).ValueGeneratedOnUpdate();
            entity.HasQueryFilter(user => user.DeletedAt == null);
        });
        modelBuilder.Entity<User>().ToTable(table =>
        {
            table.HasCheckConstraint("CK_Users_Email", "Email LIKE '%_@__%.__%' AND Email NOT LIKE '% %'");
            table.HasCheckConstraint("CK_Users_Birthday", "Birthday <= CURRENT_TIMESTAMP");
            table.HasCheckConstraint("CK_Users_Height", "Height >= 0 AND Height <= 250");
            table.HasCheckConstraint("CK_Users_Weight", "Weight >= 0 AND Weight <= 250");
        });
    }
}
