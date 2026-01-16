using FitJournal.Domain.Entities;
using FitJournal.Infra.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitJournal.Infra.Config;

internal class GoalConfig : IEntityTypeConfiguration<Goal>
{
    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder.Property(g => g.Name).IsRequired().HasMaxLength(50);
        builder.Property(g => g.Description).HasMaxLength(250);
        builder.Property(g => g.Notes).HasMaxLength(250);
        builder.Property(g => g.Type).IsRequired().HasMaxLength(15);

        builder.HasQueryFilter(g => g.DeletedAt == null);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Goals_TargetWeight", "[TargetWeight] BETWEEN 25 AND 250");
            t.HasCheckConstraint("CK_Goals_StartDate_EndDate", "[StartDate] < [EndDate]");

            t.HasTrigger(DbTriggers.GoalsBeforeRegistration);
            t.HasTrigger(DbTriggers.GoalsValidateWeight);
            t.HasTrigger(DbTriggers.GoalsValidateOverlapping);
        });
    }
}
