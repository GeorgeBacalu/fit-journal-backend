using FitJournal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitJournal.Infra.Config;

internal sealed class OAuthExchangeCodeConfig : IEntityTypeConfiguration<OAuthExchangeCode>
{
    public void Configure(EntityTypeBuilder<OAuthExchangeCode> builder)
    {
        builder.HasIndex(code => code.CodeHash).IsUnique();
        builder.HasIndex(code => code.ExpiresAt);

        builder.Property(code => code.CodeHash).IsRequired().HasMaxLength(64).IsFixedLength();
        builder.Property(code => code.TokenPayload).IsRequired().HasMaxLength(4096);
        builder.Property(code => code.ExpiresAt).IsRequired();

        builder.HasQueryFilter(code => code.DeletedAt == null);
    }
}
