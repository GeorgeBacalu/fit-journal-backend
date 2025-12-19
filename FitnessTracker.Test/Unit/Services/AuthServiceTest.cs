using AutoMapper;
using FitnessTracker.Core.Services;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Constants;
using FitnessTracker.Infra.Exceptions;
using FitnessTracker.Infra.Repositories.Interfaces;
using FitnessTracker.Test.Constants;
using FitnessTracker.Test.Integration;
using FitnessTracker.Test.Mocks.Auth;
using FitnessTracker.Test.Mocks.Users;
using FluentAssertions;
using Moq;

namespace FitnessTracker.Test.Unit.Services;

public class AuthServiceTest
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock = new();
    private readonly Mock<IMapper> mapperMock = new();
    private readonly Mock<IUserRepository> userRepositoryMock = new();
    private readonly AuthService authService;

    public AuthServiceTest()
    {
        unitOfWorkMock.Setup(mock => mock.Users).Returns(userRepositoryMock.Object);
        authService = new(unitOfWorkMock.Object, mapperMock.Object);
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
        await action.Should().ThrowAsync<BadRequestException>(ValidationErrors.AgeRestriction);

        userRepositoryMock.Verify(mock => mock.AddAsync(It.IsAny<User>(), default), Times.Never);
        unitOfWorkMock.Verify(mock => mock.CommitAsync(default), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnTokens_WhenCredentialsAreValid()
    {
        // Arrange
        AuthConfig.EnsureInitialized();

        userRepositoryMock.Setup(mock => mock.GetAsync(user => user.Email == ValidationSamples.ValidEmail)).ReturnsAsync(UserMocks.Users[0]);

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
        userRepositoryMock.Setup(mock => mock.GetAsync(user => user.Email == ValidationSamples.NonExistingEmail)).ReturnsAsync((User?)null);

        // Act
        var action = () => authService.LoginAsync(LoginRequests.NonExistingEmail, default);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>(string.Format(ErrorMessages.UserEmailNotFound, ValidationSamples.NonExistingEmail));
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowBadRequest_WhenPasswordIsIncorrect()
    {
        // Arrange
        userRepositoryMock.Setup(mock => mock.GetAsync(user => user.Email == ValidationSamples.ValidEmail)).ReturnsAsync(UserMocks.Users[0]);

        // Act
        var action = () => authService.LoginAsync(LoginRequests.WrongPassword, default);

        // Assert
        await action.Should().ThrowAsync<BadRequestException>(ErrorMessages.InvalidCredentials);
    }
}
