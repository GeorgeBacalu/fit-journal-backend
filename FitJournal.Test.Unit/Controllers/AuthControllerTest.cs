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
using Moq;

namespace FitJournal.Test.Unit.Controllers;

public class AuthControllerTest
{
    private readonly Mock<IAuthService> _authServiceMock = new();
    private readonly AuthController _authController;

    public AuthControllerTest() => _authController = new(_authServiceMock.Object);

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
            .ThrowsAsync(new BadRequestException(BusinessErrors.Users.InvalidCredentials));

        // Act
        var action = () => _authController.LoginAsync(LoginRequests.WrongPassword, default);

        // Assert
        await action.Should().ThrowAsync<BadRequestException>(BusinessErrors.Users.InvalidCredentials.Message);
    }
}
