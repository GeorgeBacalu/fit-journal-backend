using FitnessTracker.Core.Dtos.Requests.Auth;
using FitnessTracker.Core.Dtos.Responses.Auth;
using FitnessTracker.Infra.Constants;
using FitnessTracker.Infra.Context;
using FitnessTracker.Test.Constants;
using FitnessTracker.Test.Mocks.Auth;
using FitnessTracker.Test.Mocks.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace FitnessTracker.Test.Integration.Controllers;

[Collection("DbFixture")]
public class AuthControllerTest
{
    private readonly WebAppFactory factory;
    private readonly HttpClient http;

    public AuthControllerTest(DbFixture fixture) => http = (factory = new(fixture)).CreateClient();

    private async Task RunAsync(Func<Task> run)
    {
        await using var context = factory.Services.CreateScope().ServiceProvider.GetRequiredService<FitnessTrackerContext>();
        await using var transaction = await context.Database.BeginTransactionAsync(default);

        await context.Users.AddRangeAsync(UserMocks.Users, default);
        await context.SaveChangesAsync(default);

        await run();

        await transaction.RollbackAsync(default);
    }

    [Fact]
    public Task RegisterAsync_ShouldAddUser_WhenRequestIsValid()
        => RunAsync(async () =>
        {
            // Arrange

            // Act
            var response = await http.PostAsJsonAsync(ApiRoutes.Auth.Register, RegisterRequests.Valid, default);
            var responseBody = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>(default);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            responseBody.Should().ContainKey("message").WhoseValue.Should().Be(SuccessMessages.UserRegistered);
        });

    [Fact]
    public Task RegisterAsync_ShouldThrowBadRequest_WhenUserIsUnder13()
        => RunAsync(async () =>
        {
            // Arrange

            // Act
            var response = await http.PostAsJsonAsync(ApiRoutes.Auth.Register, RegisterRequests.Under13, default);
            var responseBody = await response.Content.ReadFromJsonAsync<ProblemDetails>(default);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().BeEquivalentTo(new ProblemDetails
            {
                Detail = ValidationErrors.AgeRestriction,
                Status = StatusCodes.Status400BadRequest
            });
        });

    [Theory]
    [MemberData(nameof(UserTestData.InvalidRegisterRequests), MemberType = typeof(UserTestData))]
    public Task RegisterAsync_ShouldThrowBadRequest_WhenRequestIsInvalid(RegisterRequest request, string field, string[] messages)
        => RunAsync(async () =>
        {
            // Arrange

            // Act
            var response = await http.PostAsJsonAsync(ApiRoutes.Auth.Register, request, default);
            var responseBody = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(default);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody!.Title.Should().Be("One or more validation errors occurred.");
            responseBody.Status.Should().Be(StatusCodes.Status400BadRequest);
            responseBody.Errors.Should().ContainKey(field);
            responseBody.Errors[field].Should().BeEquivalentTo(messages);
        });

    [Theory]
    [MemberData(nameof(UserTestData.DuplicatedFieldRegisterRequests), MemberType = typeof(UserTestData))]
    public Task RegisterAsync_ShouldThrowBadRequest_WhenUniqueFieldsAreDuplicated(RegisterRequest request, string title, string detail)
        => RunAsync(async () =>
        {
            // Arrange

            // Act
            var response = await http.PostAsJsonAsync(ApiRoutes.Auth.Register, request, default);
            var responseBody = await response.Content.ReadFromJsonAsync<ProblemDetails>(default);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            responseBody.Should().BeEquivalentTo(new ProblemDetails
            {
                Title = title,
                Detail = detail,
                Status = StatusCodes.Status409Conflict
            });
        });

    [Fact]
    public Task LoginAsync_ShouldReturnTokens_WhenCredentialsAreValid()
        => RunAsync(async () =>
        {
            // Arrange

            // Act
            var response = await http.PostAsJsonAsync(ApiRoutes.Auth.Login, LoginRequests.Valid, default);
            var responseBody = await response.Content.ReadFromJsonAsync<LoginResponse>(default);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBody?.AccessToken.Should().NotBeNullOrWhiteSpace();
            responseBody?.RefreshToken.Should().NotBeNullOrWhiteSpace();
        });

    [Fact]
    public Task LoginAsync_ShouldThrowNotFound_WhenEmailDoesNotExist()
        => RunAsync(async () =>
        {
            // Arrange

            // Act
            var response = await http.PostAsJsonAsync(ApiRoutes.Auth.Login, LoginRequests.NonExistingEmail, default);
            var responseBody = await response.Content.ReadFromJsonAsync<ProblemDetails>(default);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseBody.Should().BeEquivalentTo(new ProblemDetails
            {
                Detail = string.Format(ErrorMessages.UserEmailNotFound, ValidationSamples.NonExistingEmail),
                Status = StatusCodes.Status404NotFound
            });
        });

    [Fact]
    public Task LoginAsync_ShouldThrowBadRequest_WhenPasswordIsIncorrect()
        => RunAsync(async () =>
        {
            // Arrange

            // Act
            var response = await http.PostAsJsonAsync(ApiRoutes.Auth.Login, LoginRequests.WrongPassword, default);
            var responseBody = await response.Content.ReadFromJsonAsync<ProblemDetails>(default);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().BeEquivalentTo(new ProblemDetails
            {
                Detail = ErrorMessages.InvalidCredentials,
                Status = StatusCodes.Status400BadRequest
            });
        });

    [Theory]
    [MemberData(nameof(UserTestData.InvalidLoginRequests), MemberType = typeof(UserTestData))]
    public Task LoginAsync_ShouldThrowBadRequest_WhenRequestIsInvalid(LoginRequest request, string field, string[] messages)
        => RunAsync(async () =>
        {
            // Arrange

            // Act
            var response = await http.PostAsJsonAsync(ApiRoutes.Auth.Login, request, default);
            var responseBody = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(default);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody!.Title.Should().Be("One or more validation errors occurred.");
            responseBody.Status.Should().Be(StatusCodes.Status400BadRequest);
            responseBody.Errors.Should().ContainKey(field);
            responseBody.Errors[field].Should().BeEquivalentTo(messages);
        });
}
