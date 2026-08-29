using FitJournal.Api.Controllers;
using FitJournal.Core.Constants;
using FitJournal.Core.Dtos.Responses.Auth;
using FitJournal.Core.Exceptions;
using FitJournal.Core.Interfaces.Services;
using FitJournal.Test.Common.Constants;
using FitJournal.Test.Common.Mocks.Auth;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Text.Json;

namespace FitJournal.Test.Unit.Controllers;

public class AuthControllerTest
{
    private readonly Mock<IAuthService> _authServiceMock = new();
    private readonly AuthController _authController;
    private readonly TestDistributedCache _cache = new();

    public AuthControllerTest() => _authController = new(_authServiceMock.Object, new ConfigurationBuilder().Build(), _cache);

    [Fact]
    public async Task ExchangeExternalCodeAsync_ShouldConsumeCodeOnlyOnce()
    {
        var tokens = new LoginResponse { AccessToken = "access", RefreshToken = "refresh" };
        await _cache.SetStringAsync("oauth:one-time", JsonSerializer.Serialize(tokens));

        var first = (await _authController.ExchangeExternalCodeAsync(new("one-time"), default)).Result as OkObjectResult;
        var second = (await _authController.ExchangeExternalCodeAsync(new("one-time"), default)).Result as UnauthorizedResult;

        first.Should().NotBeNull();
        second.Should().NotBeNull();
    }

    [Fact]
    public async Task RegisterAsync_ShouldAddUser_WhenRequestIsValid()
    {
        // Arrange

        // Act
        var result = (await _authController.RegisterAsync(RegisterRequests.Valid, default)).Result as ObjectResult;

        // Assert
        result!.StatusCode.Should().Be(StatusCodes.Status201Created);
        result.Value.Should().BeEquivalentTo(new { Message = SuccessMessages.Users.Registered });
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowBadRequest_WhenUserIsUnder13()
    {
        // Arrange
        _authServiceMock.Setup(mock => mock.RegisterAsync(RegisterRequests.Under13, default))
            .ThrowsAsync(new BadRequestException(ValidationErrors.Users.AgeRestriction));

        // Act
        var action = () => _authController.RegisterAsync(RegisterRequests.Under13, default);

        // Assert
        await action.Should().ThrowAsync<BadRequestException>(ValidationErrors.Users.AgeRestriction.Message);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnTokens_WhenCredentialsAreValid()
    {
        // Arrange

        // Act
        var result = (await _authController.LoginAsync(LoginRequests.Valid, default)).Result as ObjectResult;

        // Assert
        result!.StatusCode.Should().Be(StatusCodes.Status200OK);
        (result.Value as LoginResponse)?.AccessToken.Should().NotBeNullOrWhiteSpace();
        (result.Value as LoginResponse)?.RefreshToken.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowNotFound_WhenEmailDoesNotExist()
    {
        // Arrange
        _authServiceMock.Setup(mock => mock.LoginAsync(LoginRequests.NonExistingEmail, default))
            .ThrowsAsync(new NotFoundException(BusinessErrors.Users.EmailNotFound(ValidationSamples.Users.NonExistingEmail)));

        // Act
        var action = () => _authController.LoginAsync(LoginRequests.NonExistingEmail, default);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>(BusinessErrors.Users.EmailNotFound(ValidationSamples.Users.NonExistingEmail).Message);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowBadRequest_WhenPasswordIsIncorrect()
    {
        // Arrange
        _authServiceMock.Setup(mock => mock.LoginAsync(LoginRequests.WrongPassword, default))
            .ThrowsAsync(new BadRequestException(BusinessErrors.Auth.InvalidCredentials));

        // Act
        var action = () => _authController.LoginAsync(LoginRequests.WrongPassword, default);

        // Assert
        await action.Should().ThrowAsync<BadRequestException>(BusinessErrors.Auth.InvalidCredentials.Message);
    }
}

internal sealed class TestDistributedCache : IDistributedCache
{
    private readonly Dictionary<string, byte[]> _values = [];
    public byte[]? Get(string key) => _values.GetValueOrDefault(key);
    public Task<byte[]?> GetAsync(string key, CancellationToken token = default) => Task.FromResult(Get(key));
    public void Refresh(string key) { }
    public Task RefreshAsync(string key, CancellationToken token = default) => Task.CompletedTask;
    public void Remove(string key) => _values.Remove(key);
    public Task RemoveAsync(string key, CancellationToken token = default) { Remove(key); return Task.CompletedTask; }
    public void Set(string key, byte[] value, DistributedCacheEntryOptions options) => _values[key] = value;
    public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default) { Set(key, value, options); return Task.CompletedTask; }
}
