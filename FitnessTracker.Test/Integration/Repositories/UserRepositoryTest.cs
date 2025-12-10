using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories;
using FitnessTracker.Test.Mocks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Test.Integration.Repositories;

[Collection("DbFixture")]
public class UserRepositoryTest(DbFixture fixture)
{
    private readonly DbContextOptions<FitnessTrackerContext> dbOptions = fixture.DbOptions;

    private async Task RunAsync(Func<UserRepository, FitnessTrackerContext, Task> run)
    {
        await using var context = new FitnessTrackerContext(dbOptions);
        await using var transaction = await context.Database.BeginTransactionAsync();

        await context.Users.AddRangeAsync(UserMock.Users);
        await context.SaveChangesAsync();
        await run(new(context), context);
        await transaction.RollbackAsync();
    }

    [Fact]
    public Task AddAsync_Test() => RunAsync(async (userRepository, context) =>
    {
        await userRepository.AddAsync(UserMock.NewUser);
        await context.SaveChangesAsync();

        (await context.Users.AsNoTracking().SingleOrDefaultAsync(user => user.Name == UserMock.NewUser.Name)).Should().NotBeNull();
    });
}
