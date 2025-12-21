using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.Infra.Config.Entities;

internal class WorkoutConfig : IEntityTypeConfiguration<Workout>
{
    public void Configure(EntityTypeBuilder<Workout> builder)
    {
        builder.HasIndex(workout => workout.Name)
            .IsUnique();

        builder.Property(workout => workout.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(workout => workout.Description)
            .HasMaxLength(250);

        builder.Property(workout => workout.Notes)
            .HasMaxLength(250);

        builder.HasQueryFilter(workout =>
            workout.DeletedAt == null &&
            workout.User != null &&
            workout.User.DeletedAt == null);

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "CK_Workouts_DurationMinutes",
                "[DurationMinutes] BETWEEN 5 AND 300");
                
            table.HasCheckConstraint(
                "CK_Workouts_StartedAt",
                "[StartedAt] <= CURRENT_TIMESTAMP");

            table.HasTrigger("TR_Workouts_BeforeUserRegistration");
        });
    }
}
