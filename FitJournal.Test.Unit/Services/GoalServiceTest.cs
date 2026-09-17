using AutoMapper;
using FitJournal.Core.Dtos.Requests.Goals;
using FitJournal.Core.Dtos.Responses.Goals;
using FitJournal.Core.Exceptions;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Core.Interfaces.Validators;
using FitJournal.Core.Services;
using FitJournal.Domain.Entities;
using FitJournal.Domain.Enums.Goals;
using FluentAssertions;
using Moq;

namespace FitJournal.Test.Unit.Services;

public class GoalServiceTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IGoalRepository> _repository = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IGoalValidator> _validator = new();
    private readonly GoalService _service;

    private static readonly Guid UserId = Guid.NewGuid();
    private static readonly Guid GoalId = Guid.NewGuid();
    private static readonly AddGoalRequest AddRequest = new()
    {
        Name = "Reach target", Type = GoalType.WeightLoss, TargetWeight = 75,
        StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
        EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1))
    };
    private static readonly EditGoalRequest EditRequest = new()
    {
        Id = GoalId, Name = "Updated target", Type = GoalType.WeightLoss, TargetWeight = 74,
        StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
        EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1))
    };

    public GoalServiceTest()
    {
        _unitOfWork.Setup(x => x.Goals).Returns(_repository.Object);
        _service = new GoalService(_unitOfWork.Object, _mapper.Object, _validator.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMappedGoal_WhenOwnedGoalExists()
    {
        var entity = new Goal { Id = GoalId, UserId = UserId, Name = "Goal" };
        var response = new GoalResponse { Id = GoalId, Name = "Goal" };
        _repository.Setup(x => x.GetByIdAsync(GoalId, UserId, default)).ReturnsAsync(entity);
        _mapper.Setup(x => x.Map<GoalResponse>(entity)).Returns(response);

        var result = await _service.GetByIdAsync(GoalId, UserId, default);

        result.Should().BeSameAs(response);
    }

    [Fact]
    public async Task GetByIdAsync_UsesUnscopedLookup_ForAdminRequest()
    {
        var entity = new Goal { Id = GoalId, UserId = UserId, Name = "Goal" };
        _repository.Setup(x => x.GetByIdAsync(GoalId, default)).ReturnsAsync(entity);
        _mapper.Setup(x => x.Map<GoalResponse>(entity)).Returns(new GoalResponse { Id = GoalId, Name = "Goal" });

        await _service.GetByIdAsync(GoalId, null, default);

        _repository.Verify(x => x.GetByIdAsync(GoalId, default), Times.Once);
        _repository.Verify(x => x.GetByIdAsync(GoalId, It.IsAny<Guid>(), default), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsNotFound_WhenGoalDoesNotExist()
    {
        _repository.Setup(x => x.GetByIdAsync(GoalId, UserId, default)).ReturnsAsync((Goal?)null);
        var action = () => _service.GetByIdAsync(GoalId, UserId, default);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AddAsync_ValidatesMapsPersistsAndCommits()
    {
        var entity = new Goal { Name = AddRequest.Name };
        _mapper.Setup(x => x.Map<Goal>(AddRequest)).Returns(entity);

        await _service.AddAsync(AddRequest, UserId, default);

        _validator.Verify(x => x.ValidateAddAsync(AddRequest, UserId, default), Times.Once);
        entity.UserId.Should().Be(UserId);
        _repository.Verify(x => x.AddAsync(entity, default), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Once);
    }

    [Fact]
    public async Task EditAsync_UpdatesTrackedGoalAndCommits()
    {
        var entity = new Goal { Id = GoalId, UserId = UserId, Name = "Old" };
        _repository.Setup(x => x.GetByIdTrackedAsync(GoalId, UserId, default)).ReturnsAsync(entity);

        await _service.EditAsync(EditRequest, UserId, default);

        _validator.Verify(x => x.ValidateEditAsync(EditRequest, UserId, default), Times.Once);
        _mapper.Verify(x => x.Map(EditRequest, entity), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Once);
    }

    [Fact]
    public async Task EditAsync_DoesNotCommit_WhenTrackedGoalIsMissing()
    {
        _repository.Setup(x => x.GetByIdTrackedAsync(GoalId, UserId, default)).ReturnsAsync((Goal?)null);
        var action = () => _service.EditAsync(EditRequest, UserId, default);
        await action.Should().ThrowAsync<NotFoundException>();
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Never);
    }

    [Fact]
    public async Task RemoveRangeAsync_RemovesAllOwnedGoalsAndCommits()
    {
        var ids = new[] { GoalId, Guid.NewGuid() };
        var request = new RemoveGoalsRequest { Ids = ids, HardDelete = true };
        _repository.Setup(x => x.CountByIdsAsync(ids, UserId, default)).ReturnsAsync(ids.Length);

        await _service.RemoveRangeAsync(request, UserId, default);

        _repository.Verify(x => x.RemoveRangeAsync(ids, UserId, true, default), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Once);
    }

    [Fact]
    public async Task RemoveRangeAsync_ThrowsWithoutMutation_WhenAnyGoalIsMissing()
    {
        var ids = new[] { GoalId, Guid.NewGuid() };
        var request = new RemoveGoalsRequest { Ids = ids };
        _repository.Setup(x => x.CountByIdsAsync(ids, UserId, default)).ReturnsAsync(1);

        var action = () => _service.RemoveRangeAsync(request, UserId, default);

        await action.Should().ThrowAsync<NotFoundException>();
        _repository.Verify(x => x.RemoveRangeAsync(It.IsAny<IEnumerable<Guid>>(), It.IsAny<Guid>(), It.IsAny<bool>(), default), Times.Never);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Never);
    }
}
