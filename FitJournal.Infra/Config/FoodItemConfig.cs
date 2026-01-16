using FitJournal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitJournal.Infra.Config;

internal class FoodItemConfig : IEntityTypeConfiguration<FoodItem>
{
    public void Configure(EntityTypeBuilder<FoodItem> builder)
    {
        builder.HasIndex(fi => fi.Name).IsUnique();

        builder.Property(fi => fi.Name).IsRequired().HasMaxLength(50);
        builder.Property(fi => fi.Category).IsRequired().HasMaxLength(15);
        builder.Property(fi => fi.Brand).IsRequired().HasMaxLength(15);

        builder.HasQueryFilter(fi => fi.DeletedAt == null);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_FoodItems_Calories", "[Calories] >= 0");
            t.HasCheckConstraint("CK_FoodItems_Protein", "[Protein] >= 0");
            t.HasCheckConstraint("CK_FoodItems_Carbs", "[Carbs] >= 0");
            t.HasCheckConstraint("CK_FoodItems_Fat", "[Fat] >= 0");
        });
    }
}
