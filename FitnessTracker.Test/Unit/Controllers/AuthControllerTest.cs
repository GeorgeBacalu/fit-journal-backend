using FitnessTracker.Api.Controllers;
using FitnessTracker.App.Services.Interfaces;
using FitnessTracker.Domain.Constants;
using FitnessTracker.Infra.Exceptions;
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
        var result = (await authController.RegisterAsync(UserMock.RegisterRequest, default)).Result as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status201Created);
        result.Value.Should().BeEquivalentTo(new { Message = SuccessMessageConstants.UserRegistered });
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowBadRequest_WhenUserIsUnder13()
    {
        // Arrange
        authServiceMock.Setup(mock => mock.RegisterAsync(UserMock.RegisterRequestUnder13, default))
            .ThrowsAsync(new BadRequestException(ErrorMessageConstants.AgeRestriction));

        // Act
        var action = () => authController.RegisterAsync(UserMock.RegisterRequestUnder13, default);

        // Assert
        await action.Should().ThrowAsync<BadRequestException>(ErrorMessageConstants.AgeRestriction);
    }
}
