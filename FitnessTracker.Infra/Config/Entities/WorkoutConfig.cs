using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.Infra.Config.Entities;

public class WorkoutConfig : IEntityTypeConfiguration<Workout>
{
    public void Configure(EntityTypeBuilder<Workout> builder)
    {
        builder.HasIndex(user => user.Name).IsUnique();

        builder.Property(user => user.Name).IsRequired().HasMaxLength(50);
        builder.Property(user => user.Description).HasMaxLength(250);
        builder.Property(user => user.Notes).HasMaxLength(250);

        builder.HasQueryFilter(user => user.DeletedAt == null);

        builder.ToTable(table =>
        {
            table.HasCheckConstraint("CK_Workout_DurationMinuts", "[DurationMinutes] BETWEEN 5 AND 300");
            table.HasCheckConstraint("CK_Workout_Date", "[StartedAt] <= CURRENT_TIMESTAMP");
            table.HasTrigger("TR_Workouts_BeforeUserRegistration");
        });
    }
}
