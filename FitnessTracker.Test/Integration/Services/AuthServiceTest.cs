using AutoMapper;
using FitnessTracker.App.Dtos.Requests.Auth;
using FitnessTracker.App.Mappers;
using FitnessTracker.App.Services;
using FitnessTracker.Domain.Constants;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Exceptions;
using FitnessTracker.Infra.Repositories;
using FitnessTracker.Test.Mocks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace FitnessTracker.Test.Integration.Services;

[Collection("DbFixture")]
public class AuthServiceTest(DbFixture fixture)
{
    private readonly DbContextOptions<FitnessTrackerContext> dbOptions = fixture.DbOptions;
    private readonly IMapper mapper = new MapperConfiguration(config => config.AddProfile<UserMapper>(), NullLoggerFactory.Instance).CreateMapper();

    private async Task RunAsync(Func<AuthService, FitnessTrackerContext, Task> run)
    {
        await using var context = new FitnessTrackerContext(dbOptions);
        await using var transaction = await context.Database.BeginTransactionAsync();

        await context.Users.AddRangeAsync(UserMock.Users);
        await context.SaveChangesAsync();
        await run(new(new UnitOfWork(context, new UserRepository(context)), mapper), context);
        await transaction.RollbackAsync();
    }

    [Fact]
    public Task RegisterAsync_ShouldAddUser_WhenRequestIsValid() => RunAsync(async (authService, context) =>
    {
        await authService.RegisterAsync(UserMock.RegisterRequest);

        (await context.Users.AsNoTracking().SingleOrDefaultAsync(user => user.Name == UserMock.NewUser.Name)).Should().NotBeNull();
    });

    [Fact]
    public Task RegisterAsync_ShouldThrowBadRequest_WhenUserIsUnder13() => RunAsync(async (authService, context) =>
        await FluentActions.Awaiting(() => authService.RegisterAsync(UserMock.RegisterRequestUnder13))
            .Should().ThrowAsync<BadRequestException>(ErrorMessageConstants.AgeRestriction));

    [Theory, MemberData(nameof(UserMock.DuplicatedFieldRegisterRequestsService), MemberType = typeof(UserMock))]
    public Task RegisterAsync_DuplicatedField_Test(RegisterRequest request, string message) => RunAsync(async (authService, context) =>
    {
        await FluentActions.Awaiting(() => authService.RegisterAsync(request)).Should().ThrowAsync<DbUpdateException>(message);

        (await context.Users.AsNoTracking().SingleOrDefaultAsync(user => user.Name == UserMock.NewUser.Name)).Should().BeNull();
    });
}
