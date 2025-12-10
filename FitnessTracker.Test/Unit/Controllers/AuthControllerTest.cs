using FitnessTracker.Api.Controllers;
using FitnessTracker.App.Dtos.Responses;
using FitnessTracker.App.Services.Interfaces;
using FitnessTracker.Domain.Constants;
using FitnessTracker.Infra.Exceptions;
using FitnessTracker.Test.Constants;
using FitnessTracker.Test.Mocks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FitnessTracker.Test.Unit.Controllers;

public class AuthControllerTest
{
    private readonly Mock<IAuthService> authServiceMock = new();
    private readonly AuthController authController;

    public AuthControllerTest() => authController = new(authServiceMock.Object);

    [Fact]
    public async Task RegisterAsync_ShouldAddUser_WhenRequestIsValid()
    {
        // Arrange

        // Act
        var result = (await authController.RegisterAsync(RegisterRequests.Valid, default)).Result as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status201Created);
        result.Value.Should().BeEquivalentTo(new { Message = SuccessMessages.UserRegistered });
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowBadRequest_WhenUserIsUnder13()
    {
        // Arrange
        authServiceMock.Setup(mock => mock.RegisterAsync(RegisterRequests.Under13, default))
            .ThrowsAsync(new BadRequestException(ErrorMessages.AgeRestriction));

        // Act
        var action = () => authController.RegisterAsync(RegisterRequests.Under13, default);

        // Assert
        await action.Should().ThrowAsync<BadRequestException>(ErrorMessages.AgeRestriction);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnTokens_WhenCredentialsAreValid()
    {
        // Arrange

        // Act
        var result = (await authController.LoginAsync(LoginRequests.Valid, default)).Result as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        (result.Value as LoginResponse)?.AccessToken.Should().NotBeNullOrWhiteSpace();
        (result.Value as LoginResponse)?.RefreshToken.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowNotFound_WhenEmailDoesNotExist()
    {
        // Arrange
        authServiceMock.Setup(mock => mock.LoginAsync(LoginRequests.NonExistingEmail, default)).ThrowsAsync(new NotFoundException(string.Format(ErrorMessages.UserEmailNotFound, ValidationSamples.NonExistingEmail)));

        // Act
        var action = () => authController.LoginAsync(LoginRequests.NonExistingEmail, default);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>(string.Format(ErrorMessages.UserEmailNotFound, ValidationSamples.NonExistingEmail));
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowBadRequest_WhenPasswordIsIncorrect()
    {
        // Arrange
        authServiceMock.Setup(mock => mock.LoginAsync(LoginRequests.WrongPassword, default)).ThrowsAsync(new BadRequestException(ErrorMessages.InvalidCredentials));

        // Act
        var action = () => authController.LoginAsync(LoginRequests.WrongPassword, default);

        // Assert
        await action.Should().ThrowAsync<BadRequestException>(ErrorMessages.InvalidCredentials);
    }
}
