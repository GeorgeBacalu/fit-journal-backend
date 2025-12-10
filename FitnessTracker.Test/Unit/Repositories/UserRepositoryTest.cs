using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories;
using FitnessTracker.Test.Mocks;
using Moq;
using Moq.EntityFrameworkCore;

namespace FitnessTracker.Test.Unit.Repositories;

public class UserRepositoryTest
{
    private readonly Mock<FitnessTrackerContext> contextMock = new();
    private readonly UserRepository userRepository;

    public UserRepositoryTest()
    {
        contextMock.Setup(mock => mock.Users).ReturnsDbSet(UserMock.Users);
        userRepository = new(contextMock.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldAddUserToContext()
    {
        // Arrange

        // Act
        await userRepository.AddAsync(UserMock.NewUser);

        // Assert
        contextMock.Verify(mock => mock.Users.AddAsync(UserMock.NewUser, default));
    }
}
