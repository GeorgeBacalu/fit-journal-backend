using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories;
using FitnessTracker.Test.Constants;
using FitnessTracker.Test.Mocks.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace FitnessTracker.Test.Unit.Repositories;

public class UserRepositoryTest
{
    private readonly Mock<FitnessTrackerContext> contextMock = new();
    private readonly UserRepository userRepository;

    public UserRepositoryTest()
    {
        contextMock.Setup(mock => mock.Users).ReturnsDbSet(UserMocks.Users);
        userRepository = new(contextMock.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldAddUserToContext()
    {
        // Arrange
        var newUser = AddUsers.NewUser();

        // Act
        await userRepository.AddAsync(newUser, default);

        // Assert
        contextMock.Verify(mock => mock.Users.AddAsync(newUser, default));
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnUser_WhenEmailExists()
    {
        // Arrange

        // Act
        var result = await userRepository.GetAsync(user => user.Email == ValidationSamples.ValidEmail, default);

        // Assert
        result.Should().Be(UserMocks.Users[0]);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenEmailDoesNotExist()
    {
        // Arrange

        // Act
        var result = await userRepository.GetAsync(user => user.Email == ValidationSamples.NonExistingEmail, default);

        // Assert
        result.Should().BeNull();
    }
}
