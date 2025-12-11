using AutoMapper;
using Azure;
using FitnessTracker.App.Dtos.Requests.Auth;
using FitnessTracker.App.Mappers;
using FitnessTracker.App.Services;
using FitnessTracker.Domain.Constants;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Exceptions;
using FitnessTracker.Infra.Repositories;
using FitnessTracker.Test.Mocks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net;

namespace FitnessTracker.Test.Integration.Services;

[Collection("DbFixture")]
public class AuthServiceTest(DbFixture fixture)
{
    private readonly DbContextOptions<FitnessTrackerContext> dbOptions = fixture.DbOptions;
    private readonly IMapper mapper = new MapperConfiguration(config
        => config.AddProfile<UserMapper>(), NullLoggerFactory.Instance).CreateMapper();

    private async Task RunAsync(Func<AuthService, FitnessTrackerContext, Task> run)
    {
        await using var context = new FitnessTrackerContext(dbOptions);
        await using var transaction = await context.Database.BeginTransactionAsync(default);

        await context.Users.AddRangeAsync(UserMocks.Users, default);
        await context.SaveChangesAsync(default);
        
        await run(new(new UnitOfWork(context, new UserRepository(context)), mapper), context);
        
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
                .SingleOrDefaultAsync(user => user.Name == AddUsers.NewUser().Name, default);
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
            await action.Should().ThrowAsync<BadRequestException>(ErrorMessages.AgeRestriction);
        });

    [Theory]
    [MemberData(nameof(UserTestData.InvalidRegisterRequests), MemberType = typeof(UserTestData))]
    public Task RegisterAsync_ShouldThrowBadRequest_WhenRequestIsInvalid(RegisterRequest request, string field, string messages)
        => RunAsync(async (authService, context) =>
        {
            // Arrange

            // Act
            var action = () => authService.RegisterAsync(request, default);

            // Assert
            var exception = await action.Should().ThrowAsync<BadRequestException>(messages);

            var user = await context.Users.AsNoTracking()
                .SingleOrDefaultAsync(user => user.Name == AddUsers.NewUser().Name, default);
            user.Should().BeNull();
        });

    [Theory]
    [MemberData(nameof(UserTestData.DuplicatedFieldRegisterRequestsService), MemberType = typeof(UserTestData))]
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
}
