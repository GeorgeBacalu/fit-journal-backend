using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.Infra.Config.Entities;

internal class WorkoutExerciseConfig : IEntityTypeConfiguration<WorkoutExercise>
{
    public void Configure(EntityTypeBuilder<WorkoutExercise> builder)
    {
        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "CK_WorkoutExercises_Sets",
                "[Sets] BETWEEN 1 AND 10");

            table.HasCheckConstraint(
                "CK_WorkoutExercises_Reps",
                "[Reps] BETWEEN 1 AND 50");

            table.HasCheckConstraint(
                "CK_WorkoutExercises_WeightUsed",
                "[WeightUsed] BETWEEN 1 AND 500");
        });

        builder.HasQueryFilter(workoutExercise => workoutExercise.DeletedAt == null);
    }
}
