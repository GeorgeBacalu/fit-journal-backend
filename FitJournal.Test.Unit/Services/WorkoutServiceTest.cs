using AutoMapper;
using FitJournal.Core.Dtos.Requests.Workouts;
using FitJournal.Core.Dtos.Responses.Workouts;
using FitJournal.Core.Exceptions;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Core.Interfaces.Validators;
using FitJournal.Core.Services;
using FitJournal.Domain.Entities;
using FluentAssertions;
using Moq;

namespace FitJournal.Test.Unit.Services;

public class WorkoutServiceTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IWorkoutRepository> _repository = new();
    private readonly Mock<IWorkoutExerciseRepository> _workoutExercises = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IWorkoutValidator> _validator = new();
    private readonly WorkoutService _service;
    private static readonly Guid UserId = Guid.NewGuid();
    private static readonly Guid WorkoutId = Guid.NewGuid();
    private static readonly AddWorkoutRequest AddRequest = new()
    {
        Name = "Push day", DurationMinutes = 60, StartedAt = DateTime.UtcNow
    };
    private static readonly EditWorkoutRequest EditRequest = new()
    {
        Id = WorkoutId, Name = "Upper body", DurationMinutes = 70, StartedAt = DateTime.UtcNow
    };

    public WorkoutServiceTest()
    {
        _unitOfWork.Setup(x => x.Workouts).Returns(_repository.Object);
        _unitOfWork.Setup(x => x.WorkoutExercises).Returns(_workoutExercises.Object);
        _service = new WorkoutService(_unitOfWork.Object, _mapper.Object, _validator.Object);
    }

    [Fact]
    public async Task GetByIdAsync_UsesOwnedLookupAndReturnsMappedWorkout()
    {
        var entity = new Workout { Id = WorkoutId, UserId = UserId, Name = "Push day" };
        var response = new WorkoutResponse { Id = WorkoutId, Name = "Push day" };
        _repository.Setup(x => x.GetByIdAsync(WorkoutId, UserId, default)).ReturnsAsync(entity);
        _mapper.Setup(x => x.Map<WorkoutResponse>(entity)).Returns(response);

        var result = await _service.GetByIdAsync(WorkoutId, UserId, default);

        result.Should().BeSameAs(response);
    }

    [Fact]
    public async Task GetByIdAsync_UsesUnscopedLookupForAdmin()
    {
        var entity = new Workout { Id = WorkoutId, Name = "Push day" };
        _repository.Setup(x => x.GetByIdAsync(WorkoutId, default)).ReturnsAsync(entity);
        _mapper.Setup(x => x.Map<WorkoutResponse>(entity)).Returns(new WorkoutResponse { Id = WorkoutId, Name = entity.Name });

        await _service.GetByIdAsync(WorkoutId, null, default);

        _repository.Verify(x => x.GetByIdAsync(WorkoutId, default), Times.Once);
        _repository.Verify(x => x.GetByIdAsync(WorkoutId, It.IsAny<Guid>(), default), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsNotFound_WhenWorkoutDoesNotExist()
    {
        _repository.Setup(x => x.GetByIdAsync(WorkoutId, UserId, default)).ReturnsAsync((Workout?)null);
        var action = () => _service.GetByIdAsync(WorkoutId, UserId, default);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AddAsync_ValidatesAssignsOwnerPersistsAndCommits()
    {
        var entity = new Workout { Name = AddRequest.Name };
        _mapper.Setup(x => x.Map<Workout>(AddRequest)).Returns(entity);

        await _service.AddAsync(AddRequest, UserId, default);

        _validator.Verify(x => x.ValidateAddAsync(AddRequest, UserId, default), Times.Once);
        entity.UserId.Should().Be(UserId);
        _repository.Verify(x => x.AddAsync(entity, default), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Once);
    }

    [Fact]
    public async Task EditAsync_UpdatesTrackedOwnedWorkoutAndCommits()
    {
        var entity = new Workout { Id = WorkoutId, UserId = UserId, Name = "Old" };
        _repository.Setup(x => x.GetByIdTrackedAsync(WorkoutId, UserId, default)).ReturnsAsync(entity);

        await _service.EditAsync(EditRequest, UserId, default);

        _validator.Verify(x => x.ValidateEditAsync(EditRequest, UserId, default), Times.Once);
        _mapper.Verify(x => x.Map(EditRequest, entity), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Once);
    }

    [Fact]
    public async Task EditAsync_DoesNotCommit_WhenOwnedWorkoutIsMissing()
    {
        _repository.Setup(x => x.GetByIdTrackedAsync(WorkoutId, UserId, default)).ReturnsAsync((Workout?)null);
        var action = () => _service.EditAsync(EditRequest, UserId, default);
        await action.Should().ThrowAsync<NotFoundException>();
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Never);
    }

    [Fact]
    public async Task RemoveRangeAsync_RemovesDependentExercisesThenWorkoutsAndCommits()
    {
        var ids = new[] { WorkoutId, Guid.NewGuid() };
        var request = new RemoveWorkoutsRequest { Ids = ids, HardDelete = true };
        _repository.Setup(x => x.CountByIdsAsync(ids, UserId, default)).ReturnsAsync(ids.Length);
        var sequence = new MockSequence();
        _workoutExercises.InSequence(sequence).Setup(x => x.RemoveRangeWorkoutsAsync(ids, true, default)).ReturnsAsync(ids.Length);
        _repository.InSequence(sequence).Setup(x => x.RemoveRangeAsync(ids, UserId, true, default)).ReturnsAsync(ids.Length);
        _unitOfWork.InSequence(sequence).Setup(x => x.CommitAsync(default)).ReturnsAsync(1);

        await _service.RemoveRangeAsync(request, UserId, default);

        _workoutExercises.Verify(x => x.RemoveRangeWorkoutsAsync(ids, true, default), Times.Once);
        _repository.Verify(x => x.RemoveRangeAsync(ids, UserId, true, default), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Once);
    }

    [Fact]
    public async Task RemoveRangeAsync_DoesNotMutate_WhenAnyOwnedWorkoutIsMissing()
    {
        var ids = new[] { WorkoutId, Guid.NewGuid() };
        var request = new RemoveWorkoutsRequest { Ids = ids };
        _repository.Setup(x => x.CountByIdsAsync(ids, UserId, default)).ReturnsAsync(1);
        var action = () => _service.RemoveRangeAsync(request, UserId, default);
        await action.Should().ThrowAsync<NotFoundException>();
        _workoutExercises.Verify(x => x.RemoveRangeWorkoutsAsync(It.IsAny<IEnumerable<Guid>>(), It.IsAny<bool>(), default), Times.Never);
        _repository.Verify(x => x.RemoveRangeAsync(It.IsAny<IEnumerable<Guid>>(), It.IsAny<Guid>(), It.IsAny<bool>(), default), Times.Never);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Never);
    }
}
