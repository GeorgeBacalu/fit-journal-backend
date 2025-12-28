using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories;
using FitnessTracker.Test.Common.Constants;
using FitnessTracker.Test.Common.Mocks.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace FitnessTracker.Test.Unit.Repositories;

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
