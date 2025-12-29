using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.Infra.Config;

internal class FoodLogConfig : IEntityTypeConfiguration<FoodLog>
{
    public void Configure(EntityTypeBuilder<FoodLog> builder)
    {
        builder.Property(fl => fl.FoodId).IsRequired();

        builder.HasQueryFilter(fl => fl.DeletedAt == null);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_FoodLogs_Date", "[Date] <= CURRENT_TIMESTAMP");
            t.HasCheckConstraint("CK_FoodLogs_Servings", "[Servings] > 0");
            t.HasCheckConstraint("CK_FoodLogs_Quantity", "[Quantity] BETWEEN 100 AND 5000");
            
            t.HasTrigger(DbTriggers.FoodLogsBeforeRegistration);
        });
    }
}
