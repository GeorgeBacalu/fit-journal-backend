using FitJournal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitJournal.Infra.Config;

public class ResetTokenConfig : IEntityTypeConfiguration<ResetToken>
{
    public void Configure(EntityTypeBuilder<ResetToken> builder)
    {
        builder.HasIndex(rt => rt.Token).IsUnique();

        builder.Property(rt => rt.Token).IsRequired().HasMaxLength(512);
        builder.Property(rt => rt.ExpiresAt).IsRequired();
        builder.Property(rt => rt.Used).IsRequired();
        builder.Property(rt => rt.UserId).IsRequired();

        builder.HasQueryFilter(rt => rt.DeletedAt == null);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_ResetTokens_Token_Length", "LEN([Token]) BETWEEN 100 AND 512 AND [Token] LIKE '%.%.%'");
            t.HasCheckConstraint("CK_ResetTokens_ExpiresAt", "[ExpiresAt] >= [CreatedAt]");
        });
    }
}
