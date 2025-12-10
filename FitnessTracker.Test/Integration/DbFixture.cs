using FitnessTracker.Infra.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Test.Integration;

public class DbFixture : IDisposable
{
    private readonly SqliteConnection _connection = new("Filename=:memory:");
    public DbContextOptions<FitnessTrackerContext> DbOptions { get; }

    public DbFixture()
    {
        _connection.Open();
        DbOptions = new DbContextOptionsBuilder<FitnessTrackerContext>().UseSqlite(_connection).Options;

        using var context = new FitnessTrackerContext(DbOptions);
        context.Database.EnsureCreated();
    }

    public void Dispose() => _connection.Dispose();
}
