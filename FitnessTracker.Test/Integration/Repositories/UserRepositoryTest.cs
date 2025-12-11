using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories;
using FitnessTracker.Test.Constants;
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

        await context.Users.AddRangeAsync(UserMocks.Users, default);
        await context.SaveChangesAsync(default);

        await run(new(context), context);

        await transaction.RollbackAsync(default);
    }

    [Fact]
    public Task AddAsync_ShouldAddUserToContext_WhenUserIsValid()
        => RunAsync(async (userRepository, context) =>
        {
            // Arrange

            // Act
            await userRepository.AddAsync(AddUsers.NewUser(), default);
            await context.SaveChangesAsync(default);

            // Assert
            var user = await context.Users.AsNoTracking()
                .SingleOrDefaultAsync(user => user.Name == AddUsers.NewUser().Name, default);
            user.Should().NotBeNull();
        });

    [Theory]
    [MemberData(nameof(UserTestData.InvalidAddUsers), MemberType = typeof(UserTestData))]
    public Task AddAsync_ShouldThrowDbUpdate_WhenConstraintIsViolated(User user, string message)
        => RunAsync(async (userRepository, context) =>
        {
            // Arrange

            // Act
            var action = async () =>
            {
                await userRepository.AddAsync(user, default);
                await context.SaveChangesAsync(default);
            };

            // Assert
            var exception = await action.Should().ThrowAsync<DbUpdateException>();
            exception.Which.InnerException!.Message.Should().Contain(message);
        });

    [Fact]
    public Task GetByEmailAsync_ShouldReturnUser_WhenEmailExists()
        => RunAsync(async (userRepository, context) =>
        {
            // Arrange

            // Act
            var result = await userRepository.GetByEmailAsync(ValidationSamples.ValidEmail, default);

            // Assert
            result.Should().Be(UserMocks.Users[0]);
        });

    [Fact]
    public Task GetByEmailAsync_ShouldReturnNull_WhenEmailDoesNotExist()
        => RunAsync(async (userRepository, context) =>
        {
            // Arrange

            // Act
            var result = await userRepository.GetByEmailAsync(ValidationSamples.NonExistingEmail, default);

            // Assert
            result.Should().BeNull();
        });
}
