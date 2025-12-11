using AutoMapper;
using FitnessTracker.App.Services;
using FitnessTracker.Domain.Constants;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Config;
using FitnessTracker.Infra.Exceptions;
using FitnessTracker.Infra.Repositories.Interfaces;
using FitnessTracker.Test.Constants;
using FitnessTracker.Test.Mocks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;

namespace FitnessTracker.Test.Unit.Services;

public class AuthServiceTest
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock = new();
    private readonly Mock<IMapper> mapperMock = new();
    private readonly Mock<IUserRepository> userRepositoryMock = new();
    private readonly AuthService authService;

    public AuthServiceTest()
    {
        unitOfWorkMock.Setup(mock => mock.UserRepository).Returns(userRepositoryMock.Object);
        authService = new(unitOfWorkMock.Object, mapperMock.Object);

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        AppConfig.Init(new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Auth:Issuer"] = "https://localhost:5000/",
            ["Auth:Audience"] = "http://localhost:4200/",
            ["Auth:Secret"] = "00000000000000000000000000000000",
            ["Auth:AccessTokenLifetimeMinutes"] = "15",
            ["Auth:RefreshTokenLifetimeDays"] = "30"
        }).Build());
    }

    [Fact]
    public async Task RegisterAsync_ShouldAddUser_WhenRequestIsValid()
    {
        // Arrange
        var newUser = AddUsers.NewUser();
        mapperMock.Setup(mock => mock.Map<User>(RegisterRequests.Valid)).Returns(newUser);

        // Act
        await authService.RegisterAsync(RegisterRequests.Valid, default);

        // Assert
        userRepositoryMock.Verify(mock => mock.AddAsync(newUser, default));
        unitOfWorkMock.Verify(mock => mock.CommitAsync(default));
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowBadRequest_WhenUserIsUnder13()
    {
        // Arrange

        // Act
        var action = () => authService.RegisterAsync(RegisterRequests.Under13, default);

        // Assert
        await action.Should().ThrowAsync<BadRequestException>(ErrorMessages.AgeRestriction);

        userRepositoryMock.Verify(mock => mock.AddAsync(AddUsers.NewUser(), default), Times.Never);
        unitOfWorkMock.Verify(mock => mock.CommitAsync(default), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnTokens_WhenCredentialsAreValid()
    {
        // Arrange
        userRepositoryMock.Setup(mock => mock.GetByEmailAsync(ValidationSamples.ValidEmail)).ReturnsAsync(UserMocks.Users[0]);

        // Act
        var result = await authService.LoginAsync(LoginRequests.Valid, default);

        // Assert
        result.AccessToken.Should().NotBeNullOrWhiteSpace();
        result.RefreshToken.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowNotFound_WhenEmailDoesNotExist()
    {
        // Arrange
        userRepositoryMock.Setup(mock => mock.GetByEmailAsync(ValidationSamples.NonExistingEmail)).ReturnsAsync((User?)null);

        // Act
        var action = () => authService.LoginAsync(LoginRequests.NonExistingEmail, default);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>(string.Format(ErrorMessages.UserEmailNotFound, ValidationSamples.NonExistingEmail));
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowBadRequest_WhenPasswordIsIncorrect()
    {
        // Arrange
        userRepositoryMock.Setup(mock => mock.GetByEmailAsync(ValidationSamples.ValidEmail)).ReturnsAsync(UserMocks.Users[0]);

        // Act
        var action = () => authService.LoginAsync(LoginRequests.WrongPassword, default);

        // Assert
        await action.Should().ThrowAsync<BadRequestException>(ErrorMessages.InvalidCredentials);
    }
}
