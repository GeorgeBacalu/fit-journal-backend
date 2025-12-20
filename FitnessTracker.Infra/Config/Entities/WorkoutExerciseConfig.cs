using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.Infra.Config.Entities;

internal class WorkoutExerciseConfig : IEntityTypeConfiguration<WorkoutExercise>
{
    public void Configure(EntityTypeBuilder<WorkoutExercise> builder)
    {
        builder.Property(workoutExercise => workoutExercise.WeightUsed)
            .HasMaxLength(50);

        builder.HasQueryFilter(workoutExercise => workoutExercise.DeletedAt == null);

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "CK_WorkoutExercise_Sets",
                "[Sets] > 0");

            table.HasCheckConstraint(
                "CK_WorkoutExercise_Reps",
                "[Reps] > 0");
        });
    }
}
