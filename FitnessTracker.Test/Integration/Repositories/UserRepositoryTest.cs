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
        await using var transaction = await context.Database.BeginTransactionAsync(default);

        await context.Users.AddRangeAsync(UserMock.Users, default);
        await context.SaveChangesAsync(default);
        
        await run(new(context), context);
        
        await transaction.RollbackAsync(default);
    }

    [Fact]
    public Task AddAsync_Test()
        => RunAsync(async (userRepository, context) =>
        {
            // Arrange

            // Act
            await userRepository.AddAsync(UserMock.NewUser, default);
            await context.SaveChangesAsync(default);

            // Assert
            var user = await context.Users.AsNoTracking()
                .SingleOrDefaultAsync(user => user.Name == UserMock.NewUser.Name, default);
            user.Should().NotBeNull();
        });
}
