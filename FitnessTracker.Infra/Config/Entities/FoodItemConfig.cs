using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.Infra.Config.Entities;

internal class FoodItemConfig : IEntityTypeConfiguration<FoodItem>
{
    public void Configure(EntityTypeBuilder<FoodItem> builder)
    {
        builder.HasIndex(foodItem => foodItem.Name)
            .IsUnique();

        builder.Property(foodItem => foodItem.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasQueryFilter(foodItem => foodItem.DeletedAt == null);

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "CK_FoodItem_Calories",
                "[Calories] >= 0");

            table.HasCheckConstraint(
                "CK_FoodItem_Protein",
                "[Protein] >= 0");

            table.HasCheckConstraint(
                "CK_FoodItem_Carbs",
                "[Carbs] >= 0");

            table.HasCheckConstraint(
                "CK_FoodItem_Fat",
                "[Fat] >= 0");
        });
    }
}
