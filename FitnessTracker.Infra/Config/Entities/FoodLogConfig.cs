using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.Infra.Config.Entities;

internal class FoodLogConfig : IEntityTypeConfiguration<FoodLog>
{
    public void Configure(EntityTypeBuilder<FoodLog> builder)
    {
        builder.HasQueryFilter(foodLog =>
            foodLog.DeletedAt == null &&
            foodLog.User != null &&
            foodLog.User.DeletedAt == null);

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "CK_FoodLog_Date",
                "[Date] <= CURRENT_TIMESTAMP"
            );

            table.HasCheckConstraint(
                "CK_FoodLog_Servings",
                "[Servings] > 0"
            );

            table.HasCheckConstraint(
                "CK_FoodLog_Quantity",
                "[Quantity] BETWEEN 100 AND 5000"
            );
        });
    }
}
