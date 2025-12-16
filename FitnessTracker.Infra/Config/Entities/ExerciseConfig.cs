using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.Infra.Config.Entities;

public class ExerciseConfig : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.HasIndex(user => user.Name).IsUnique();

        builder.Property(user => user.Name).IsRequired().HasMaxLength(50);
        builder.Property(user => user.MuscleGroup).IsRequired().HasConversion<string>().HasMaxLength(10);
        builder.Property(user => user.DifficultyLevel).IsRequired().HasConversion<string>().HasMaxLength(15);

        builder.HasQueryFilter(user => user.DeletedAt == null);
    }
}
