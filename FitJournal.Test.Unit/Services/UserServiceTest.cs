using AutoMapper;
using FitJournal.Core.Dtos.Requests.Users;
using FitJournal.Core.Dtos.Responses.Users;
using FitJournal.Core.Exceptions;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Core.Interfaces.Validators;
using FitJournal.Core.Services;
using FitJournal.Domain.Entities;
using FitJournal.Domain.Enums.Users;
using FluentAssertions;
using Moq;

namespace FitJournal.Test.Unit.Services;

public class UserServiceTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IUserRepository> _repository = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IUserValidator> _validator = new();
    private readonly UserService _service;
    private static readonly Guid UserId = Guid.NewGuid();
    private static readonly EditUserRequest EditRequest = new()
    {
        Name = "Alex", Email = "alex@example.com", Phone = "+40123456789",
        Birthday = new DateOnly(1990, 1, 1), Height = 180, Weight = 80, Gender = Gender.Male
    };

    public UserServiceTest()
    {
        _unitOfWork.Setup(x => x.Users).Returns(_repository.Object);
        _service = new UserService(_unitOfWork.Object, _mapper.Object, _validator.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMappedUser_WhenUserExists()
    {
        var entity = new User { Id = UserId, Name = "Alex", Email = "alex@example.com", PasswordHash = "hash", Phone = "+40123456789" };
        var response = new UserResponse { Id = UserId, Name = "Alex", Email = entity.Email, Phone = entity.Phone };
        _repository.Setup(x => x.GetByIdAsync(UserId, default)).ReturnsAsync(entity);
        _mapper.Setup(x => x.Map<UserResponse>(entity)).Returns(response);

        var result = await _service.GetByIdAsync(UserId, default);

        result.Should().BeSameAs(response);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsNotFound_WhenUserDoesNotExist()
    {
        _repository.Setup(x => x.GetByIdAsync(UserId, default)).ReturnsAsync((User?)null);
        var action = () => _service.GetByIdAsync(UserId, default);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task EditAsync_ValidatesUpdatesTrackedUserAndCommits()
    {
        var entity = new User { Id = UserId, Name = "Old", Email = "old@example.com", PasswordHash = "hash", Phone = "+40111111111" };
        _repository.Setup(x => x.GetByIdTrackedAsync(UserId, default)).ReturnsAsync(entity);

        await _service.EditAsync(EditRequest, UserId, default);

        _validator.Verify(x => x.ValidateEditAsync(EditRequest, UserId, default), Times.Once);
        _mapper.Verify(x => x.Map(EditRequest, entity), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Once);
    }

    [Fact]
    public async Task EditAsync_DoesNotCommit_WhenUserDoesNotExist()
    {
        _repository.Setup(x => x.GetByIdTrackedAsync(UserId, default)).ReturnsAsync((User?)null);
        var action = () => _service.EditAsync(EditRequest, UserId, default);
        await action.Should().ThrowAsync<NotFoundException>();
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Never);
    }
}
