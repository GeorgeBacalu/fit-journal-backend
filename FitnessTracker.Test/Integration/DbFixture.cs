using FitnessTracker.Infra.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Test.Integration;

public sealed class DbFixture : IDisposable
{
    private readonly SqliteConnection connection = new("Filename=:memory:");
    public DbContextOptions<FitnessTrackerContext> DbOptions { get; private set; }

    public DbFixture()
    {
        connection.Open();
        DbOptions = new DbContextOptionsBuilder<FitnessTrackerContext>().UseSqlite(connection).Options;

        using var context = new FitnessTrackerContext(DbOptions);
        context.Database.EnsureCreated();
    }

    public void Dispose() => connection.Dispose();
}
