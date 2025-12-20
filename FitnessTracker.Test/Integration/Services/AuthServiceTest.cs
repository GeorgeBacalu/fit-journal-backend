using AutoMapper;
using FitnessTracker.Core.Dtos.Requests.Auth;
using FitnessTracker.Core.Mappers;
using FitnessTracker.Core.Services;
using FitnessTracker.Infra.Constants;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Exceptions;
using FitnessTracker.Infra.Repositories;
using FitnessTracker.Test.Constants;
using FitnessTracker.Test.Mocks.Auth;
using FitnessTracker.Test.Mocks.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace FitnessTracker.Test.Integration.Services;

[Collection("DbFixture")]
public class AuthServiceTest(DbFixture fixture)
{
    private readonly IMapper mapper = new MapperConfiguration(config
        => config.AddProfile<UserMapper>(), NullLoggerFactory.Instance).CreateMapper();

    private async Task RunAsync(Func<AuthService, FitnessTrackerContext, Task> run)
    {
        await using var context = new FitnessTrackerContext(fixture.DbOptions);
        await using var transaction = await context.Database.BeginTransactionAsync(default);

        await context.Users.AddRangeAsync(UserMocks.Users, default);
        await context.SaveChangesAsync(default);

        await run(new(new UnitOfWork(context), mapper), context);

        await transaction.RollbackAsync(default);
    }

    [Fact]
    public Task RegisterAsync_ShouldAddUser_WhenRequestIsValid()
        => RunAsync(async (authService, context) =>
        {
            // Arrange

            // Act
            await authService.RegisterAsync(RegisterRequests.Valid, default);

            // Assert
            var user = await context.Users.AsNoTracking()
                .SingleOrDefaultAsync(user => user.Name == RegisterRequests.Valid.Name, default);
            user.Should().NotBeNull();
        });

    [Fact]
    public Task RegisterAsync_ShouldThrowBadRequest_WhenUserIsUnder13()
        => RunAsync(async (authService, context) =>
        {
            // Arrange

            // Act
            var action = () => authService.RegisterAsync(RegisterRequests.Under13, default);

            // Assert
            await action.Should().ThrowAsync<BadRequestException>(ValidationErrors.Users.AgeRestriction);
        });

    [Theory]
    [MemberData(nameof(UserTestData.DuplicatedFieldRegisterRequests), MemberType = typeof(UserTestData))]
    public Task RegisterAsync_ShouldThrowBadRequest_WhenUniqueFieldsAreDuplicated(RegisterRequest request, string message)
        => RunAsync(async (authService, context) =>
        {
            // Arrange

            // Act
            var action = () => authService.RegisterAsync(request, default);

            // Assert
            await action.Should().ThrowAsync<DbUpdateException>(message);

            var user = await context.Users.AsNoTracking()
                .SingleOrDefaultAsync(user => user.Name == AddUsers.NewUser().Name, default);
            user.Should().BeNull();
        });

    [Fact]
    public Task LoginAsync_ShouldReturnTokens_WhenCredentialsAreValid()
        => RunAsync(async (authService, context) =>
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
    public Task LoginAsync_ShouldThrowNotFound_WhenEmailDoesNotExist()
        => RunAsync(async (authService, context) =>
        {
            // Arrange

            // Act
            var action = () => authService.LoginAsync(LoginRequests.NonExistingEmail, default);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>(string.Format(ErrorMessages.Users.IdNotFound, ValidationSamples.NonExistingEmail));
        });

    [Fact]
    public Task LoginAsync_ShouldThrowBadRequest_WhenPasswordIsIncorrect()
        => RunAsync(async (authService, context) =>
        {
            // Arrange

            // Act
            var action = () => authService.LoginAsync(LoginRequests.WrongPassword, default);

            // Assert
            await action.Should().ThrowAsync<BadRequestException>(ErrorMessages.Users.InvalidCredentials);
        });
}
