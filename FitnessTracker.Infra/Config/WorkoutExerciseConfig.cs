using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.Infra.Config;

internal class WorkoutExerciseConfig : IEntityTypeConfiguration<WorkoutExercise>
{
    public void Configure(EntityTypeBuilder<WorkoutExercise> builder)
    {
        builder.HasIndex(we => new { we.WorkoutId, we.ExerciseId }).IsUnique();

        builder.HasQueryFilter(we => we.DeletedAt == null);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_WorkoutExercises_Sets", "[Sets] BETWEEN 1 AND 10");
            t.HasCheckConstraint("CK_WorkoutExercises_Reps", "[Reps] BETWEEN 1 AND 50");
            t.HasCheckConstraint("CK_WorkoutExercises_WeightUsed", "[WeightUsed] BETWEEN 0 AND 500");
        });
    }
}
