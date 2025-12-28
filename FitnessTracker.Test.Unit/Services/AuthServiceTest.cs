using AutoMapper;
using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Interfaces.Validators;
using FitnessTracker.Core.Services;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Test.Common.Config;
using FitnessTracker.Test.Common.Constants;
using FitnessTracker.Test.Common.Mocks.Auth;
using FitnessTracker.Test.Common.Mocks.Users;
using FluentAssertions;
using Moq;

namespace FitnessTracker.Test.Unit.Services;

public class AuthServiceTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IUserValidator> _userValidatorMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly AuthService _authService;

    public AuthServiceTest()
    {
        _unitOfWorkMock.Setup(mock => mock.Users).Returns(_userRepositoryMock.Object);
        _authService = new(_unitOfWorkMock.Object, _mapperMock.Object, _userValidatorMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_ShouldAddUser_WhenRequestIsValid()
    {
        // Arrange
        var newUser = AddUsers.NewUser();
        _mapperMock.Setup(mock => mock.Map<User>(RegisterRequests.Valid)).Returns(newUser);

        // Act
        await _authService.RegisterAsync(RegisterRequests.Valid, default);

        // Assert
        _userRepositoryMock.Verify(mock => mock.AddAsync(newUser, default));
        _unitOfWorkMock.Verify(mock => mock.CommitAsync(default));
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowBadRequest_WhenUserIsUnder13()
    {
        // Arrange

        // Act
        var action = () => _authService.RegisterAsync(RegisterRequests.Under13, default);

        // Assert
        await action.Should().ThrowAsync<BadRequestException>(ValidationErrors.Users.AgeRestriction);

        _userRepositoryMock.Verify(mock => mock.AddAsync(It.IsAny<User>(), default), Times.Never);
        _unitOfWorkMock.Verify(mock => mock.CommitAsync(default), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnTokens_WhenCredentialsAreValid()
    {
        // Arrange
        AuthConfig.EnsureInitialized();

        _userRepositoryMock.Setup(mock => mock.GetAsync(user => user.Email == ValidationSamples.Users.ValidEmail, default)).ReturnsAsync(UserMocks.Users[0]);

        // Act
        var result = await _authService.LoginAsync(LoginRequests.Valid, default);

        // Assert
        result.AccessToken.Should().NotBeNullOrWhiteSpace();
        result.RefreshToken.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowNotFound_WhenEmailDoesNotExist()
    {
        // Arrange
        _userRepositoryMock.Setup(mock => mock.GetAsync(user => user.Email == ValidationSamples.Users.NonExistingEmail, default)).ReturnsAsync((User?)null);

        // Act
        var action = () => _authService.LoginAsync(LoginRequests.NonExistingEmail, default);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>(string.Format(BusinessErrors.Users.EmailNotFound, ValidationSamples.Users.NonExistingEmail));
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowBadRequest_WhenPasswordIsIncorrect()
    {
        // Arrange
        _userRepositoryMock.Setup(mock => mock.GetAsync(user => user.Email == ValidationSamples.Users.ValidEmail, default)).ReturnsAsync(UserMocks.Users[0]);

        // Act
        var action = () => _authService.LoginAsync(LoginRequests.WrongPassword, default);

        // Assert
        await action.Should().ThrowAsync<BadRequestException>(BusinessErrors.Users.InvalidCredentials);
    }
}
