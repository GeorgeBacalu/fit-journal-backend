using FitJournal.Core.Dtos.Responses.Auth;
using FitJournal.Infra.Context;
using FitJournal.Infra.Services;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FitJournal.Test.Integration.Services;

public class ExternalAuthCodeStoreTest
{
    [Fact]
    public async Task RedeemAsync_ConsumesPersistedCodeExactlyOnce()
    {
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;
        await using var db = new AppDbContext(options);
        await db.Database.EnsureCreatedAsync();
        var store = new ExternalAuthCodeStore(db);
        var expected = new LoginResponse { AccessToken = "access", RefreshToken = "refresh" };

        var code = await store.StoreAsync(expected, TimeSpan.FromMinutes(1), default);
        var first = await store.RedeemAsync(code, default);
        var replay = await store.RedeemAsync(code, default);

        code.Should().HaveLength(64);
        first.Should().BeEquivalentTo(expected);
        replay.Should().BeNull();
        (await db.OAuthExchangeCodes.AsNoTracking().SingleAsync()).ConsumedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task RedeemAsync_RejectsExpiredOrMalformedCodes()
    {
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;
        await using var db = new AppDbContext(options);
        await db.Database.EnsureCreatedAsync();
        var store = new ExternalAuthCodeStore(db);
        var code = await store.StoreAsync(new() { AccessToken = "access", RefreshToken = "refresh" }, TimeSpan.FromSeconds(-1), default);

        (await store.RedeemAsync(code, default)).Should().BeNull();
        (await store.RedeemAsync("not-a-valid-code", default)).Should().BeNull();
    }
}
