using FitJournal.Core.Constants;
using FitJournal.Core.Dtos.Requests.Auth;
using FitJournal.Core.Dtos.Responses.Auth;
using FitJournal.Infra.Context;
using FitJournal.Test.Common.Constants;
using FitJournal.Test.Common.Mocks.Auth;
using FitJournal.Test.Common.Mocks.Users;
using FitJournal.Test.Integration.Config;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace FitJournal.Test.Integration.Controllers;

[Collection("DbFixture")]
public class AuthControllerTest
{
    private readonly WebAppFactory _factory;
    private readonly HttpClient _http;

    public AuthControllerTest(DbFixture fixture)
    {
        _factory = new(fixture);
        _http = _factory.CreateClient();
    }

    private async Task RunAsync(Func<Task> run)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await using var transaction = await db.Database.BeginTransactionAsync(default);

        await db.Users.AddRangeAsync(UserMocks.Users, default);
        await db.SaveChangesAsync(default);

        await run();

        await transaction.RollbackAsync(default);
    }

    [Fact]
    public Task RegisterAsync_ShouldAddUser_WhenRequestIsValid() =>
        RunAsync(async () =>
        {
            // Arrange

            // Act
            var response = await _http.PostAsJsonAsync(ApiRoutes.Auth.Register, RegisterRequests.Valid, default);
            var responseBody = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>(default);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            responseBody.Should().ContainKey("message").WhoseValue.Should().Be(SuccessMessages.Users.Registered);
        });

    [Fact]
    public Task RegisterAsync_ShouldThrowBadRequest_WhenUserIsUnder13() =>
        RunAsync(async () =>
        {
            // Arrange

            // Act
            var response = await _http.PostAsJsonAsync(ApiRoutes.Auth.Register, RegisterRequests.Under13, default);
            var responseBody = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(default);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody!.Errors[nameof(RegisterRequest.Birthday)]
                .Should().Contain(ValidationErrors.Users.AgeRestriction.Message);
        });

    [Theory]
    [MemberData(nameof(UserTestData.InvalidRegisterRequests), MemberType = typeof(UserTestData))]
    public Task RegisterAsync_ShouldThrowBadRequest_WhenRequestIsInvalid(RegisterRequest request, string field, string[] messages) =>
        RunAsync(async () =>
        {
            // Arrange

            // Act
            var response = await _http.PostAsJsonAsync(ApiRoutes.Auth.Register, request, default);
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
    public Task RegisterAsync_ShouldThrowBadRequest_WhenUniqueFieldsAreDuplicated(RegisterRequest request, FitJournal.Core.Results.Error detail) =>
        RunAsync(async () =>
        {
            // Arrange

            // Act
            var response = await _http.PostAsJsonAsync(ApiRoutes.Auth.Register, request, default);
            var responseBody = await response.Content.ReadFromJsonAsync<ProblemDetails>(default);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().BeEquivalentTo(new ProblemDetails
            {
                Title = detail.Code,
                Detail = detail.Message,
            }, options => options.Excluding(problem => problem.Status));
        });

    [Fact]
    public Task LoginAsync_ShouldReturnTokens_WhenCredentialsAreValid() =>
        RunAsync(async () =>
        {
            // Arrange

            // Act
            var response = await _http.PostAsJsonAsync(ApiRoutes.Auth.Login, LoginRequests.Valid, default);
            var responseBody = await response.Content.ReadFromJsonAsync<LoginResponse>(default);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBody?.AccessToken.Should().NotBeNullOrWhiteSpace();
            responseBody?.RefreshToken.Should().NotBeNullOrWhiteSpace();
        });

    [Fact]
    public Task RefreshToken_ShouldNotAuthorizeApiRequests() =>
        RunAsync(async () =>
        {
            var loginResponse = await _http.PostAsJsonAsync(ApiRoutes.Auth.Login, LoginRequests.Valid, default);
            var tokens = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>(default);
            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{ApiRoutes.Users.Base}/{UserMocks.Users[0].Id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens!.RefreshToken);

            var response = await _http.SendAsync(request, default);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        });

    [Fact]
    public Task LoginAsync_ShouldThrowNotFound_WhenEmailDoesNotExist() =>
        RunAsync(async () =>
        {
            // Arrange

            // Act
            var response = await _http.PostAsJsonAsync(ApiRoutes.Auth.Login, LoginRequests.NonExistingEmail, default);
            var responseBody = await response.Content.ReadFromJsonAsync<ProblemDetails>(default);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseBody!.Title.Should().Be(BusinessErrors.Users.EmailNotFound(ValidationSamples.Users.NonExistingEmail).Code);
            responseBody.Detail.Should().Be(BusinessErrors.Users.EmailNotFound(ValidationSamples.Users.NonExistingEmail).Message);
        });

    [Fact]
    public Task LoginAsync_ShouldThrowBadRequest_WhenPasswordIsIncorrect() =>
        RunAsync(async () =>
        {
            // Arrange

            // Act
            var response = await _http.PostAsJsonAsync(ApiRoutes.Auth.Login, LoginRequests.WrongPassword);
            var responseBody = await response.Content.ReadFromJsonAsync<ProblemDetails>(default);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody!.Title.Should().Be(BusinessErrors.Auth.InvalidCredentials.Code);
            responseBody.Detail.Should().Be(BusinessErrors.Auth.InvalidCredentials.Message);
        });

    [Theory]
    [MemberData(nameof(UserTestData.InvalidLoginRequests), MemberType = typeof(UserTestData))]
    public Task LoginAsync_ShouldThrowBadRequest_WhenRequestIsInvalid(LoginRequest request, string field, string[] messages) =>
        RunAsync(async () =>
        {
            // Arrange

            // Act
            var response = await _http.PostAsJsonAsync(ApiRoutes.Auth.Login, request, default);
            var responseBody = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(default);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody!.Title.Should().Be("One or more validation errors occurred.");
            responseBody.Status.Should().Be(StatusCodes.Status400BadRequest);
            responseBody.Errors.Should().ContainKey(field);
            responseBody.Errors[field].Should().BeEquivalentTo(messages);
        });
}
