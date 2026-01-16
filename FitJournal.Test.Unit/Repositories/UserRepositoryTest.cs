using FitJournal.Domain.Entities;
using FitJournal.Infra.Context;
using FitJournal.Infra.Repositories;
using FitJournal.Test.Common.Constants;
using FitJournal.Test.Common.Mocks.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace FitJournal.Test.Unit.Repositories;

public class UserRepositoryTest
{
    private readonly Mock<AppDbContext> _dbMock = new();
    private readonly UserRepository _userRepository;

    public UserRepositoryTest()
    {
        _dbMock.Setup(mock => mock.Set<User>()).ReturnsDbSet(UserMocks.Users);
        _userRepository = new(_dbMock.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldAddUserToDb_WhenUserIsValid()
    {
        // Arrange
        var newUser = AddUsers.NewUser();

        // Act
        await _userRepository.AddAsync(newUser, default);

        // Assert
        _dbMock.Verify(mock => mock.Set<User>().AddAsync(newUser, default));
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnUser_WhenEmailExists()
    {
        // Arrange

        // Act
        var result = await _userRepository.GetAsync(user => user.Email == ValidationSamples.Users.ValidEmail, default);

        // Assert
        result.Should().Be(UserMocks.Users[0]);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenEmailDoesNotExist()
    {
        // Arrange

        // Act
        var result = await _userRepository.GetAsync(user => user.Email == ValidationSamples.Users.NonExistingEmail, default);

        // Assert
        result.Should().BeNull();
    }
}
