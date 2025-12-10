using FitnessTracker.App.Dtos.Requests.Auth;
using FitnessTracker.Domain.Constants;
using FitnessTracker.Infra.Context;
using FitnessTracker.Test.Mocks;
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

        await context.Users.AddRangeAsync(UserMock.Users, default);
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
            var response = await http.PostAsJsonAsync($"{ApiConstants.ApiAuth}/register", UserMock.RegisterRequest, default);
            var responseBody = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>(default);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            responseBody.Should().ContainKey("message").WhoseValue.Should().Be(SuccessMessageConstants.UserRegistered);
        });

    [Fact]
    public Task RegisterAsync_ShouldThrowBadRequest_WhenUserIsUnder13()
        => RunAsync(async () =>
        {
            // Arrange

            // Act
            var response = await http.PostAsJsonAsync($"{ApiConstants.ApiAuth}/register", UserMock.RegisterRequestUnder13, default);
            var responseBody = await response.Content.ReadFromJsonAsync<ProblemDetails>(default);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody.Should().BeEquivalentTo(new ProblemDetails
            {
                Title = "Bad request",
                Detail = ErrorMessageConstants.AgeRestriction,
                Status = StatusCodes.Status400BadRequest
            });
        });

    [Theory, MemberData(nameof(UserMock.InvalidRegisterRequests), MemberType = typeof(UserMock))]
    public Task RegisterAsync_ShouldThrowBadRequest_WhenRequestIsInvalid(RegisterRequest request, string field, string[] messages)
        => RunAsync(async () =>
        {
            // Arrange

            // Act
            var response = await http.PostAsJsonAsync($"{ApiConstants.ApiAuth}/register", request, default);
            var responseBody = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(default);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseBody!.Title.Should().Be("One or more validation errors occurred.");
            responseBody.Status.Should().Be(StatusCodes.Status400BadRequest);
            responseBody.Errors.Should().ContainKey(field);
            responseBody.Errors[field].Should().BeEquivalentTo(messages);
        });

    [Theory, MemberData(nameof(UserMock.DuplicatedFieldRegisterRequests), MemberType = typeof(UserMock))]
    public Task RegisterAsync_ShouldThrowBadRequest_WhenUniqueFieldsAreDuplicated(RegisterRequest request, string title, string detail)
        => RunAsync(async () =>
        {
            // Arrange

            // Act
            var response = await http.PostAsJsonAsync($"{ApiConstants.ApiAuth}/register", request, default);
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
}
