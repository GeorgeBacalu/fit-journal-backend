using FitJournal.Api.Extensions;
using FitJournal.Core;
using FitJournal.Core.Constants;
using FitJournal.Core.Dtos.Requests.Auth;
using FitJournal.Core.Exceptions;
using FitJournal.Core.Interfaces.Services;
using FitJournal.Core.Mappers;
using FitJournal.Infra;
using FitJournal.Infra.Context;
using FitJournal.Test.Common.Config;
using FitJournal.Test.Common.Constants;
using FitJournal.Test.Common.Mocks.Auth;
using FitJournal.Test.Common.Mocks.Users;
using FitJournal.Test.Integration.Config;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FitJournal.Test.Integration.Services;

[Collection("DbFixture")]
public class AuthServiceTest(DbFixture fixture)
{
    private async Task RunAsync(Func<IAuthService, AppDbContext, Task> run)
    {
        await using var scope = new ServiceCollection()
            .AddCore().AddValidators()
            .AddInfra(options => options.UseSqlite(fixture.Connection))
            .AddAutoMapper(_ => { }, typeof(UserMapper).Assembly)
            .BuildServiceProvider()
            .CreateAsyncScope();

        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await using var transaction = await db.Database.BeginTransactionAsync(default);

        await db.Users.AddRangeAsync(UserMocks.Users, default);
        await db.SaveChangesAsync(default);

        await run(authService, db);

        await transaction.RollbackAsync(default);
    }

    [Fact]
    public Task RegisterAsync_ShouldAddUser_WhenRequestIsValid() =>
        RunAsync(async (authService, db) =>
        {
            // Arrange

            // Act
            await authService.RegisterAsync(RegisterRequests.Valid, default);

            // Assert
            var user = await db.Users.AsNoTracking()
                .SingleOrDefaultAsync(user => user.Name == RegisterRequests.Valid.Name, default);
            user.Should().NotBeNull();
        });

    [Fact]
    public Task RegisterAsync_ShouldThrowBadRequest_WhenUserIsUnder13() =>
        RunAsync(async (authService, db) =>
        {
            // Arrange

            // Act
            var action = () => authService.RegisterAsync(RegisterRequests.Under13, default);

            // Assert
            await action.Should().ThrowAsync<BadRequestException>(ValidationErrors.Users.AgeRestriction.Message);
        });

    [Theory]
    [MemberData(nameof(UserTestData.DuplicatedFieldRegisterRequests), MemberType = typeof(UserTestData))]
    public Task RegisterAsync_ShouldThrowBadRequest_WhenUniqueFieldsAreDuplicated(RegisterRequest request, string message) =>
        RunAsync(async (authService, db) =>
        {
            // Arrange

            // Act
            var action = () => authService.RegisterAsync(request, default);

            // Assert
            await action.Should().ThrowAsync<DbUpdateException>(message);

            var user = await db.Users.AsNoTracking()
                .SingleOrDefaultAsync(user => user.Name == AddUsers.NewUser().Name, default);
            user.Should().BeNull();
        });

    [Fact]
    public Task LoginAsync_ShouldReturnTokens_WhenCredentialsAreValid() =>
        RunAsync(async (authService, db) =>
        {
            // Arrange
            AuthConfig.EnsureInitialized();

            // Act
            var result = await authService.LoginAsync(LoginRequests.Valid, default);

            // Assert
            result.AccessToken.Should().NotBeNullOrWhiteSpace();
            result.RefreshToken.Should().NotBeNullOrWhiteSpace();
        });

    [Fact]
    public Task LoginAsync_ShouldThrowNotFound_WhenEmailDoesNotExist() =>
        RunAsync(async (authService, db) =>
        {
            // Arrange

            // Act
            var action = () => authService.LoginAsync(LoginRequests.NonExistingEmail, default);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>(BusinessErrors.Users.EmailNotFound(ValidationSamples.Users.NonExistingEmail).Message);
        });

    [Fact]
    public Task LoginAsync_ShouldThrowBadRequest_WhenPasswordIsIncorrect() =>
        RunAsync(async (authService, db) =>
        {
            // Arrange

            // Act
            var action = () => authService.LoginAsync(LoginRequests.WrongPassword, default);

            // Assert
            await action.Should().ThrowAsync<BadRequestException>(BusinessErrors.Users.InvalidCredentials.Message);
        });
}
