using FitJournal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitJournal.Infra.Config;

internal class ExerciseConfig : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.HasIndex(e => e.Name).IsUnique();

        builder.Property(e => e.Name).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Description).HasMaxLength(250);
        builder.Property(e => e.Notes).HasMaxLength(250);
        builder.Property(e => e.MuscleGroup).IsRequired().HasMaxLength(15);
        builder.Property(e => e.DifficultyLevel).IsRequired().HasMaxLength(15);

        builder.HasQueryFilter(e => e.DeletedAt == null);
    }
}
