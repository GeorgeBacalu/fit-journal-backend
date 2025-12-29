using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.Infra.Config;

internal class ProgressLogConfig : IEntityTypeConfiguration<ProgressLog>
{
    public void Configure(EntityTypeBuilder<ProgressLog> builder)
    {
        builder.HasIndex(pl => new { pl.UserId, pl.Date }).IsUnique();

        builder.HasQueryFilter(pl => pl.DeletedAt == null);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_ProgressLogs_Date", "[Date] <= CURRENT_TIMESTAMP");
            t.HasCheckConstraint("CK_ProgressLogs_Weight", "[Weight] BETWEEN 25 AND 250");
            t.HasCheckConstraint("CK_ProgressLogs_BodyFat", "[BodyFat] BETWEEN 2 AND 60");
            t.HasCheckConstraint("CK_ProgressLogs_WaistCm", "[WaistCm] BETWEEN 30 AND 250");
            t.HasCheckConstraint("CK_ProgressLogs_ChestCm", "[ChestCm] BETWEEN 30 AND 200");
            t.HasCheckConstraint("CK_ProgressLogs_ArmsCm", "[ArmsCm] BETWEEN 10 AND 100");

            t.HasTrigger(DbTriggers.ProgressLogsBeforeRegistration);
        });
    }
}
