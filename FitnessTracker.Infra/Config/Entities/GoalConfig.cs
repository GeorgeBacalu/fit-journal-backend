using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.Infra.Config.Entities;

internal class GoalConfig : IEntityTypeConfiguration<Goal>
{
    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder.Property(goal => goal.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(goal => goal.Description)
            .HasMaxLength(250);

        builder.Property(goal => goal.Type)
            .IsRequired()
            .HasMaxLength(15);

        builder.HasQueryFilter(goal =>
            goal.DeletedAt == null &&
            goal.User != null &&
            goal.User.DeletedAt == null);

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "CK_Goals_TargetWeight",
                "[TargetWeight] BETWEEN 25 AND 250");

            table.HasCheckConstraint(
                "CK_Goals_Dates",
                "[StartDate] < [EndDate]");

            table.HasTrigger("TR_Goals_BeforeUserRegistration");
            table.HasTrigger("TR_Goals_ValidateWeight");
            table.HasTrigger("TR_Goals_ValidateOverlapping");
        });
    }
}
