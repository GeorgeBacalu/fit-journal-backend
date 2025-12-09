using FitnessTracker.Api.Controllers;
using FitnessTracker.App.Services.Interfaces;
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
    public async Task RegisterAsync_Test()
    {
        var result = (await authController.RegisterAsync(UserMock.RegisterRequest, default)).Result as ObjectResult;

        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status201Created);
        result.Value.Should().BeEquivalentTo(new { Message = "User registered successfully" });
    }
}
