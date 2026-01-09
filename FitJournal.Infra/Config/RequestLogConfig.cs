using FitJournal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitJournal.Infra.Config;

internal class RequestLogConfig : IEntityTypeConfiguration<RequestLog>
{
    public void Configure(EntityTypeBuilder<RequestLog> builder)
    {
        builder.Property(rl => rl.Duration).IsRequired();

        builder.Property(rl => rl.ExceptionType).HasMaxLength(128);
        builder.Property(rl => rl.ExceptionMessage).HasMaxLength(512);
        builder.Property(rl => rl.ExceptionStackTrace).HasColumnType("nvarchar(max)");
        builder.Property(rl => rl.InnerExceptionType).HasMaxLength(128);
        builder.Property(rl => rl.InnerExceptionMessage).HasMaxLength(512);
        builder.Property(rl => rl.InnerExceptionStackTrace).HasColumnType("nvarchar(max)");

        builder.Property(rl => rl.Host).IsRequired().HasMaxLength(128);
        builder.Property(rl => rl.Ip).IsRequired().HasMaxLength(45);
        builder.Property(rl => rl.Language).IsRequired().HasMaxLength(64);

        builder.Property(rl => rl.Method).HasMaxLength(8);
        builder.Property(rl => rl.Path).IsRequired().HasMaxLength(128);
        builder.Property(rl => rl.QueryString).HasMaxLength(512);

        builder.Property(rl => rl.RemoteIp).IsRequired();

        builder.Property(rl => rl.RequestBody).IsRequired().HasMaxLength(10240);
        builder.Property(rl => rl.RequestBodySize).IsRequired();
        builder.Property(rl => rl.RequestHeader).IsRequired().HasColumnType("nvarchar(max)");

        builder.Property(rl => rl.ResponseBody).IsRequired().HasMaxLength(10240);
        builder.Property(rl => rl.ResponseBodySize).IsRequired();
        builder.Property(rl => rl.ResponseHeader).IsRequired().HasColumnType("nvarchar(max)");
        builder.Property(rl => rl.ResponseStatus).IsRequired();

        builder.HasIndex(rl => rl.CreatedAt);
        builder.HasIndex(rl => rl.Duration);
        builder.HasIndex(rl => rl.ResponseStatus);
        builder.HasIndex(rl => rl.UserId);
        builder.HasIndex(rl => new { rl.Method, rl.Path });

        builder.HasQueryFilter(rl => rl.DeletedAt == null);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_RequestLogs_Duration", "[Duration] >= 0");
            t.HasCheckConstraint("CK_RequestLogs_RequestBodySize", "[RequestBodySize] >= 0");
            t.HasCheckConstraint("CK_RequestLogs_ResponseBodySize", "[ResponseBodySize] >= 0");
            t.HasCheckConstraint("CK_RequestLogs_ResponseStatus", "[ResponseStatus] BETWEEN 100 AND 599");
        });
    }
}
