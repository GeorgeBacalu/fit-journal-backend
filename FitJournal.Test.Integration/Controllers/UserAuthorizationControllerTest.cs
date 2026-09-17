using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FitJournal.Core.Dtos.Requests.Auth;
using FitJournal.Core.Dtos.Requests.Users;
using FitJournal.Core.Dtos.Responses.Auth;
using FitJournal.Core.Dtos.Responses.Users;
using FitJournal.Domain.Entities;
using FitJournal.Domain.Enums.Users;
using FitJournal.Infra.Context;
using FitJournal.Test.Common.Constants;
using FitJournal.Test.Common.Mocks.Auth;
using FitJournal.Test.Common.Mocks.Users;
using FitJournal.Test.Integration.Config;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace FitJournal.Test.Integration.Controllers;

[Collection("DbFixture")]
public class UserAuthorizationControllerTest
{
    private static readonly Guid AdminId = Guid.Parse("13fb36da-ff6f-41bb-af84-828fe2ef6b29");
    private readonly WebAppFactory _factory;

    public UserAuthorizationControllerTest(DbFixture fixture) => _factory = new(fixture);

    [Fact]
    public Task GetByIdAsync_ShouldForbidAnotherUsersProfile() =>
        RunAsync(async client =>
        {
            await AuthenticateAsync(client, LoginRequests.Valid);

            var response = await client.GetAsync($"{ApiRoutes.Users.Base}/{UserMocks.Users[1].Id}");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        });

    [Fact]
    public Task GetByIdAsync_ShouldReturnOwnProfile() =>
        RunAsync(async client =>
        {
            await AuthenticateAsync(client, LoginRequests.Valid);

            var response = await client.GetAsync($"{ApiRoutes.Users.Base}/{UserMocks.Users[0].Id}");
            var profile = await response.Content.ReadFromJsonAsync<UserResponse>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            profile!.Id.Should().Be(UserMocks.Users[0].Id);
            profile.Email.Should().Be(UserMocks.Users[0].Email);
        });

    [Fact]
    public Task GetAllAsync_ShouldForbidNonAdmin() =>
        RunAsync(async client =>
        {
            await AuthenticateAsync(client, LoginRequests.Valid);

            var response = await client.PostAsJsonAsync($"{ApiRoutes.Users.Base}/all", new UserPaginationRequest());

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        });

    [Fact]
    public Task GetAllAsync_ShouldReturnUsersForAdmin() =>
        RunAsync(async client =>
        {
            await AuthenticateAsync(client, new LoginRequest
            {
                Email = "admin@fitjournal.test",
                Password = "AdminPassword0!"
            });

            var response = await client.PostAsJsonAsync($"{ApiRoutes.Users.Base}/all", new UserPaginationRequest());
            var users = await response.Content.ReadFromJsonAsync<UsersResponse>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            users!.Users.Should().Contain(user => user.Id == AdminId && user.Role == Role.Admin);
        });

    [Fact]
    public Task GetByIdAsync_ShouldAllowAdminToInspectUser() =>
        RunAsync(async client =>
        {
            await AuthenticateAsync(client, new LoginRequest
            {
                Email = "admin@fitjournal.test",
                Password = "AdminPassword0!"
            });

            var response = await client.GetAsync($"{ApiRoutes.Users.Base}/{UserMocks.Users[0].Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        });

    private async Task RunAsync(Func<HttpClient, Task> run)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await using var transaction = await db.Database.BeginTransactionAsync();

        await db.Users.AddRangeAsync(UserMocks.Users.Select(CloneUser));
        await db.Users.AddAsync(new User
        {
            Id = AdminId,
            Name = "FitJournal Admin",
            Email = "admin@fitjournal.test",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("AdminPassword0!"),
            Phone = "+40-700-000-000",
            Birthday = new DateOnly(1990, 1, 1),
            Height = 175,
            Weight = 70,
            Gender = Gender.Unknown,
            Role = Role.Admin
        });
        await db.SaveChangesAsync();

        using var client = _factory.CreateClient();
        await run(client);
        await transaction.RollbackAsync();
    }

    private static async Task AuthenticateAsync(HttpClient client, LoginRequest request)
    {
        var response = await client.PostAsJsonAsync(ApiRoutes.Auth.Login, request);
        var tokens = await response.Content.ReadFromJsonAsync<LoginResponse>();
        response.EnsureSuccessStatusCode();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens!.AccessToken);
    }

    private static User CloneUser(User source) => new()
    {
        Id = source.Id,
        Name = source.Name,
        Email = source.Email,
        PasswordHash = source.PasswordHash,
        Phone = source.Phone,
        Birthday = source.Birthday,
        Height = source.Height,
        Weight = source.Weight,
        Gender = source.Gender,
        Role = source.Role,
        CreatedAt = source.CreatedAt,
        UpdatedAt = source.UpdatedAt,
        DeletedAt = source.DeletedAt
    };
}
