using FitJournal.Infra.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FitJournal.Test.Integration.Config;

public sealed class DbFixture : IAsyncLifetime
{
    public SqliteConnection Connection { get; } = new("Filename=:memory:");
    public DbContextOptions<AppDbContext> DbOptions { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await Connection.OpenAsync();
        DbOptions = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(Connection).Options;

        await using var db = new AppDbContext(DbOptions);
        await db.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync() => await Connection.DisposeAsync();
}
