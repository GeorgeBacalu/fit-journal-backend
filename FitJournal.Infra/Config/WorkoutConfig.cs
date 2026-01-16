using FitJournal.Domain.Entities;
using FitJournal.Infra.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitJournal.Infra.Config;

internal class WorkoutConfig : IEntityTypeConfiguration<Workout>
{
    public void Configure(EntityTypeBuilder<Workout> builder)
    {
        builder.HasIndex(w => new { w.UserId, w.Name }).IsUnique();

        builder.Property(w => w.Name).IsRequired().HasMaxLength(50);
        builder.Property(w => w.Description).HasMaxLength(250);
        builder.Property(w => w.Notes).HasMaxLength(250);

        builder.HasQueryFilter(w => w.DeletedAt == null);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Workouts_DurationMinutes", "[DurationMinutes] BETWEEN 5 AND 300");
            t.HasCheckConstraint("CK_Workouts_StartedAt", "[StartedAt] <= CURRENT_TIMESTAMP");

            t.HasTrigger(DbTriggers.WorkoutsBeforeRegistration);
        });
    }
}
