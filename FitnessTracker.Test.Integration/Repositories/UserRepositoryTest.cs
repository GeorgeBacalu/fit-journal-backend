using FitnessTracker.Core;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Mappers;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra;
using FitnessTracker.Infra.Context;
using FitnessTracker.Test.Common.Constants;
using FitnessTracker.Test.Common.Mocks.Users;
using FitnessTracker.Test.Integration.Config;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessTracker.Test.Integration.Repositories;

[Collection("DbFixture")]
public class UserRepositoryTest(DbFixture fixture)
{
    private async Task RunAsync(Func<IUserRepository, AppDbContext, Task> run)
    {
        await using var scope = new ServiceCollection()
            .AddCore().AddValidators()
            .AddInfra(options => options.UseSqlite(fixture.Connection))
            .AddAutoMapper(_ => { }, typeof(UserMapper).Assembly)
            .BuildServiceProvider()
            .CreateAsyncScope();

        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await using var transaction = await db.Database.BeginTransactionAsync(default);

        await db.Users.AddRangeAsync(UserMocks.Users, default);
        await db.SaveChangesAsync(default);

        await run(userRepository, db);

        await transaction.RollbackAsync(default);
    }

    [Fact]
    public Task AddAsync_ShouldAddUserToDb_WhenUserIsValid() =>
        RunAsync(async (userRepository, db) =>
        {
            // Arrange
            var newUser = AddUsers.NewUser();

            // Act
            await userRepository.AddAsync(newUser, default);
            await db.SaveChangesAsync(default);

            // Assert
            var user = await db.Users.AsNoTracking()
                .SingleOrDefaultAsync(user => user.Name == newUser.Name, default);
            user.Should().NotBeNull();
        });

    [Theory]
    [MemberData(nameof(UserTestData.InvalidAddUsers), MemberType = typeof(UserTestData))]
    public Task AddAsync_ShouldThrowDbUpdate_WhenConstraintIsViolated(User user, string message) =>
        RunAsync(async (userRepository, db) =>
        {
            // Arrange

            // Act
            var action = async () =>
            {
                await userRepository.AddAsync(user, default);
                await db.SaveChangesAsync(default);
            };

            // Assert
            var exception = await action.Should().ThrowAsync<DbUpdateException>();
            exception.Which.InnerException!.Message.Should().Contain(message);
        });

    [Fact]
    public Task GetByEmailAsync_ShouldReturnUser_WhenEmailExists() =>
        RunAsync(async (userRepository, db) =>
        {
            // Arrange

            // Act
            var result = await userRepository.GetAsync(user => user.Email == ValidationSamples.Users.ValidEmail, default);

            // Assert
            result.Should().BeEquivalentTo(UserMocks.Users[0]);
        });

    [Fact]
    public Task GetByEmailAsync_ShouldReturnNull_WhenEmailDoesNotExist() =>
        RunAsync(async (userRepository, db) =>
        {
            // Arrange

            // Act
            var result = await userRepository.GetAsync(user => user.Email == ValidationSamples.Users.NonExistingEmail, default);

            // Assert
            result.Should().BeNull();
        });
}
