using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Context;

public class FitnessTrackerContext : DbContext
{
    public FitnessTrackerContext() { }
    public FitnessTrackerContext(DbContextOptions<FitnessTrackerContext> options) : base(options) { }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(user => user.Name).IsUnique();
            entity.HasIndex(user => user.Email).IsUnique();

            entity.Property(user => user.Name).IsRequired().HasMaxLength(50);
            entity.Property(user => user.Email).IsRequired().HasMaxLength(50);
            entity.Property(user => user.Password).IsRequired().HasMaxLength(25);
            entity.Property(user => user.Phone).IsRequired().HasMaxLength(15);

            entity.Property(user => user.Gender).HasConversion<string>().HasMaxLength(8);
            entity.Property(user => user.Role).HasConversion<string>().HasMaxLength(6);

            entity.Property(user => user.UpdatedAt).ValueGeneratedOnUpdate();
        });
        modelBuilder.Entity<User>().ToTable(table =>
        {
            table.HasCheckConstraint("CK_User_Email", "[Email] LIKE '%_@__%.__%'");
            table.HasCheckConstraint("CK_Users_Birthday", "[Birthday] <= GETDATE()");
            table.HasCheckConstraint("CK_Users_Height", "[Height] > 0 AND [Height] < 250");
            table.HasCheckConstraint("CK_Users_Weight", "[Weight] > 0 AND [Weight] < 250");
        });
        modelBuilder.Entity<User>().HasQueryFilter(user => user.DeletedAt == null);
    }
}
