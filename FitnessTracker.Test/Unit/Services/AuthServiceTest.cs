using AutoMapper;
using FitnessTracker.App.Services;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Repositories.Interfaces;
using FitnessTracker.Test.Mocks;
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
        unitOfWorkMock.Setup(mock => mock.UserRepository).Returns(userRepositoryMock.Object);
        authService = new(unitOfWorkMock.Object, mapperMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_Test()
    {
        mapperMock.Setup(mock => mock.Map<User>(UserMock.RegisterRequest)).Returns(UserMock.NewUser);

        await authService.RegisterAsync(UserMock.RegisterRequest, default);

        userRepositoryMock.Verify(mock => mock.AddAsync(UserMock.NewUser, default));
        unitOfWorkMock.Verify(mock => mock.CommitAsync());
    }
}
