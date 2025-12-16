using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.Infra.Config.Entities;

public class GoalConfig : IEntityTypeConfiguration<Goal>
{
    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder.Property(goal => goal.Title).IsRequired().HasMaxLength(50);
        builder.Property(goal => goal.Description).HasMaxLength(250);

        builder.Property(goal => goal.Type).IsRequired().HasConversion<string>().HasMaxLength(15);

        builder.HasQueryFilter(goal =>
            goal.DeletedAt == null &&
            goal.User != null &&
            goal.User.DeletedAt == null);

        builder.ToTable(table =>
        {
            table.HasCheckConstraint("CK_Goals_TargetWeight", "[TargetWeight] > 0");
            table.HasCheckConstraint("CK_Goals_Dates", "[StartDate] < [EndDate]");
            table.HasTrigger("TR_Goals_BeforeUserRegistration");
            table.HasTrigger("TR_Goals_ValitateWeight");
            table.HasTrigger("TR_Goals_ValidateOverlapping");
        });
    }
}
