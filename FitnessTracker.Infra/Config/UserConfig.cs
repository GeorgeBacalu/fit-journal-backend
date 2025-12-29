using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.Infra.Config;

internal class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.Name).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.Name).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(50);
        builder.Property(u => u.PasswordHash).IsRequired().IsFixedLength().HasMaxLength(60);
        builder.Property(u => u.Phone).IsRequired().HasMaxLength(20);
        builder.Property(u => u.Gender).IsRequired().HasMaxLength(8);
        builder.Property(u => u.Role).IsRequired().HasMaxLength(6);

        builder.HasQueryFilter(u => u.DeletedAt == null);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Users_Email", "[Email] LIKE '%_@__%.__%'");
            t.HasCheckConstraint("CK_Users_Birthday", "[Birthday] <= CURRENT_TIMESTAMP");
            t.HasCheckConstraint("CK_Users_AgeRestriction", "DATEDIFF(year, [Birthday], CURRENT_TIMESTAMP) >= 13");
            t.HasCheckConstraint("CK_Users_Height", "[Height] BETWEEN 120 AND 250");
            t.HasCheckConstraint("CK_Users_Weight", "[Weight] BETWEEN 25 AND 250");
        });
    }
}
