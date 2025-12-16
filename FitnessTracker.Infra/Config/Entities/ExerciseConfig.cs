using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.Infra.Config.Entities;

public class ExerciseConfig : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.HasIndex(exercise => exercise.Name).IsUnique();

        builder.Property(exercise => exercise.Name).IsRequired().HasMaxLength(50);
        builder.Property(exercise => exercise.Description).HasMaxLength(250);
        builder.Property(exercise => exercise.Notes).HasMaxLength(250);

        builder.Property(exercise => exercise.MuscleGroup).IsRequired().HasConversion<string>().HasMaxLength(10);
        builder.Property(exercise => exercise.DifficultyLevel).IsRequired().HasConversion<string>().HasMaxLength(15);

        builder.HasQueryFilter(exercise => exercise.DeletedAt == null);
    }
}
