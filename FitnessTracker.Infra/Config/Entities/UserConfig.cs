using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.Infra.Config.Entities;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(user => user.Name).IsUnique();
        builder.HasIndex(user => user.Email).IsUnique();

        builder.Property(user => user.Name).IsRequired().HasMaxLength(50);
        builder.Property(user => user.Email).IsRequired().HasMaxLength(50);
        builder.Property(user => user.PasswordHash).IsRequired().IsFixedLength().HasMaxLength(60);
        builder.Property(user => user.Phone).IsRequired().HasMaxLength(20);

        builder.Property(user => user.Gender).IsRequired().HasConversion<string>().HasMaxLength(8);
        builder.Property(user => user.Role).IsRequired().HasConversion<string>().HasMaxLength(6);

        builder.HasQueryFilter(user => user.DeletedAt == null);

        builder.ToTable(table =>
        {
            table.HasCheckConstraint("CK_Users_Email", "[Email] LIKE '%_@__%.__%'");
            table.HasCheckConstraint("CK_Users_Birthday", "[Birthday] <= CURRENT_TIMESTAMP");
            table.HasCheckConstraint("CK_Users_Height", "[Height] BETWEEN 120 AND 250");
            table.HasCheckConstraint("CK_Users_Weight", "[Weight] BETWEEN 25 AND 250");
        });
    }
}
