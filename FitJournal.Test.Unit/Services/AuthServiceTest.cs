using AutoMapper;
using FitJournal.Core.Constants;
using FitJournal.Core.Exceptions;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Core.Interfaces.Services;
using FitJournal.Core.Interfaces.Validators;
using FitJournal.Core.Dtos.Requests.Email;
using FitJournal.Core.Services;
using FitJournal.Domain.Entities;
using FitJournal.Test.Common.Config;
using FitJournal.Test.Common.Constants;
using FitJournal.Test.Common.Mocks.Auth;
using FitJournal.Test.Common.Mocks.Users;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace FitJournal.Test.Unit.Services;

public class AuthServiceTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IEmailService> _emailServiceMock = new();
    private readonly Mock<IUserValidator> _userValidatorMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IResetTokenRepository> _resetTokenRepositoryMock = new();
    private readonly AuthService _authService;

    public AuthServiceTest()
    {
        AuthConfig.EnsureInitialized();
        _unitOfWorkMock.Setup(mock => mock.Users).Returns(_userRepositoryMock.Object);
        _unitOfWorkMock.Setup(mock => mock.ResetTokens).Returns(_resetTokenRepositoryMock.Object);
        _authService = new(_unitOfWorkMock.Object, _mapperMock.Object, _emailServiceMock.Object, _userValidatorMock.Object);
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
        _userValidatorMock.Setup(mock => mock.ValidateRegisterAsync(RegisterRequests.Under13, default))
            .ThrowsAsync(new BadRequestException(ValidationErrors.Users.AgeRestriction));

        // Act
        var action = () => _authService.RegisterAsync(RegisterRequests.Under13, default);

        // Assert
        await action.Should().ThrowAsync<BadRequestException>(ValidationErrors.Users.AgeRestriction.Message);

        _userRepositoryMock.Verify(mock => mock.AddAsync(It.IsAny<User>(), default), Times.Never);
        _unitOfWorkMock.Verify(mock => mock.CommitAsync(default), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnTokens_WhenCredentialsAreValid()
    {
        // Arrange
        _userRepositoryMock.Setup(mock => mock.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), default)).ReturnsAsync(UserMocks.Users[0]);

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
        _userRepositoryMock.Setup(mock => mock.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), default)).ReturnsAsync((User?)null);

        // Act
        var action = () => _authService.LoginAsync(LoginRequests.NonExistingEmail, default);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>(BusinessErrors.Users.EmailNotFound(ValidationSamples.Users.NonExistingEmail).Message);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowBadRequest_WhenPasswordIsIncorrect()
    {
        // Arrange
        _userRepositoryMock.Setup(mock => mock.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), default)).ReturnsAsync(UserMocks.Users[0]);

        // Act
        var action = () => _authService.LoginAsync(LoginRequests.WrongPassword, default);

        // Assert
        await action.Should().ThrowAsync<BadRequestException>(BusinessErrors.Auth.InvalidCredentials.Message);
    }

    [Fact]
    public async Task ForgotPasswordAsync_ShouldSendFrontendResetLink()
    {
        _userRepositoryMock
            .Setup(mock => mock.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), default))
            .ReturnsAsync(UserMocks.Users[0]);
        _emailServiceMock
            .Setup(mock => mock.GeneratePasswordResetEmail(It.IsAny<FitJournal.Core.Dtos.Common.Email.PasswordResetEmail>()))
            .Returns("reset email");

        await _authService.ForgotPasswordAsync(
            new FitJournal.Core.Dtos.Requests.Auth.ForgotPasswordRequest { Email = UserMocks.Users[0].Email },
            default);

        _resetTokenRepositoryMock.Verify(mock => mock.AddAsync(
            It.Is<ResetToken>(reset => reset.UserId == UserMocks.Users[0].Id), default));
        _emailServiceMock.Verify(mock => mock.GeneratePasswordResetEmail(
            It.Is<FitJournal.Core.Dtos.Common.Email.PasswordResetEmail>(email =>
                email.ResetLink.StartsWith("https://app.fitjournal.test/reset-password?token="))));
        _emailServiceMock.Verify(mock => mock.SendAsync(
            It.Is<SendEmailRequest>(email => email.To == UserMocks.Users[0].Email && email.Body == "reset email"),
            default));
    }
}
