using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.Infra.Config.Entities;

internal class MeasurementLogConfig : IEntityTypeConfiguration<MeasurementLog>
{
    public void Configure(EntityTypeBuilder<MeasurementLog> builder)
    {
        builder.HasIndex(measurementLog => new { measurementLog.UserId, measurementLog.Date })
            .IsUnique();

        builder.Property(measurementLog => measurementLog.BodyFatPercentage)
            .HasPrecision(4, 2);

        builder.HasQueryFilter(measurementLog =>
            measurementLog.DeletedAt == null &&
            measurementLog.User != null &&
            measurementLog.User.DeletedAt == null);

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "CK_MeasurementLogs_Date",
                "[Date] <= CURRENT_TIMESTAMP");

            table.HasCheckConstraint(
                "CK_MeasurementLogs_Weight",
                "[Weight] BETWEEN 25 AND 250");

            table.HasCheckConstraint(
                "CK_MeasurementLogs_BodyFatPercentage",
                "[BodyFatPercentage] BETWEEN 2 AND 60");

            table.HasCheckConstraint(
                "CK_MeasurementLogs_WaistCircumference",
                "[WaistCircumference] BETWEEN 30 AND 250");

            table.HasCheckConstraint(
                "CK_MeasurementLogs_ChestCircumference",
                "[ChestCircumference] BETWEEN 30 AND 200");

            table.HasCheckConstraint(
                "CK_MeasurementLogs_ArmsCircumference",
                "[ArmsCircumference] BETWEEN 10 AND 100");

            table.HasTrigger("TR_MeasurementLogs_BeforeUserRegistration");
        });
    }
}
