using AutoMapper;
using FitJournal.Core.Dtos.Requests.WorkoutExercises;
using FitJournal.Core.Exceptions;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Core.Services;
using FitJournal.Domain.Entities;
using FluentAssertions;
using Moq;

namespace FitJournal.Test.Unit.Services;

public class WorkoutExerciseServiceTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IWorkoutRepository> _workouts = new();
    private readonly Mock<IExerciseRepository> _exercises = new();
    private readonly Mock<IWorkoutExerciseRepository> _entries = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly WorkoutExerciseService _service;

    private static readonly Guid UserId = Guid.NewGuid();
    private static readonly Guid WorkoutId = Guid.NewGuid();
    private static readonly Guid ExerciseId = Guid.NewGuid();
    private static readonly AddWorkoutExerciseRequest AddRequest = new()
    {
        WorkoutId = WorkoutId, ExerciseId = ExerciseId, Sets = 3, Reps = 10, WeightUsed = 50
    };

    public WorkoutExerciseServiceTest()
    {
        _unitOfWork.Setup(x => x.Workouts).Returns(_workouts.Object);
        _unitOfWork.Setup(x => x.Exercises).Returns(_exercises.Object);
        _unitOfWork.Setup(x => x.WorkoutExercises).Returns(_entries.Object);
        _service = new WorkoutExerciseService(_unitOfWork.Object, _mapper.Object);
    }

    private void SetupValidReferences()
    {
        _workouts.Setup(x => x.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Workout, bool>>>(), default)).ReturnsAsync(true);
        _exercises.Setup(x => x.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Exercise, bool>>>(), default)).ReturnsAsync(true);
    }

    [Fact]
    public async Task AddAsync_PersistsUniqueEntryAndCommits()
    {
        SetupValidReferences();
        _entries.Setup(x => x.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<WorkoutExercise, bool>>>(), default)).ReturnsAsync(false);
        var entity = new WorkoutExercise { WorkoutId = WorkoutId, ExerciseId = ExerciseId };
        _mapper.Setup(x => x.Map<WorkoutExercise>(AddRequest)).Returns(entity);

        await _service.AddAsync(AddRequest, UserId, default);

        _entries.Verify(x => x.AddAsync(entity, default), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Once);
    }

    [Fact]
    public async Task AddAsync_RejectsWorkoutNotOwnedByUser()
    {
        _workouts.Setup(x => x.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Workout, bool>>>(), default)).ReturnsAsync(false);
        var action = () => _service.AddAsync(AddRequest, UserId, default);
        await action.Should().ThrowAsync<NotFoundException>();
        _entries.Verify(x => x.AddAsync(It.IsAny<WorkoutExercise>(), default), Times.Never);
    }

    [Fact]
    public async Task AddAsync_RejectsMissingExercise()
    {
        _workouts.Setup(x => x.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Workout, bool>>>(), default)).ReturnsAsync(true);
        _exercises.Setup(x => x.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Exercise, bool>>>(), default)).ReturnsAsync(false);
        var action = () => _service.AddAsync(AddRequest, UserId, default);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AddAsync_RejectsDuplicateExerciseInWorkout()
    {
        SetupValidReferences();
        _entries.Setup(x => x.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<WorkoutExercise, bool>>>(), default)).ReturnsAsync(true);
        var action = () => _service.AddAsync(AddRequest, UserId, default);
        await action.Should().ThrowAsync<BadRequestException>();
    }

    [Fact]
    public async Task EditAsync_RejectsEntryBelongingToDifferentPair()
    {
        SetupValidReferences();
        var request = new EditWorkoutExerciseRequest
        {
            Id = Guid.NewGuid(), WorkoutId = WorkoutId, ExerciseId = ExerciseId, Sets = 4, Reps = 8, WeightUsed = 60
        };
        _entries.Setup(x => x.GetByIdTrackedAsync(request.Id, default)).ReturnsAsync(new WorkoutExercise
        {
            WorkoutId = Guid.NewGuid(), ExerciseId = ExerciseId
        });

        var action = () => _service.EditAsync(request, UserId, default);
        await action.Should().ThrowAsync<BadRequestException>();
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Never);
    }

    [Fact]
    public async Task EditAsync_MapsMatchingEntryAndCommits()
    {
        SetupValidReferences();
        var request = new EditWorkoutExerciseRequest
        {
            Id = Guid.NewGuid(), WorkoutId = WorkoutId, ExerciseId = ExerciseId, Sets = 4, Reps = 8, WeightUsed = 60
        };
        var entity = new WorkoutExercise { WorkoutId = WorkoutId, ExerciseId = ExerciseId };
        _entries.Setup(x => x.GetByIdTrackedAsync(request.Id, default)).ReturnsAsync(entity);

        await _service.EditAsync(request, UserId, default);

        _mapper.Verify(x => x.Map(request, entity), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Once);
    }

    [Fact]
    public async Task RemoveRangeAsync_RemovesOnlyWhenEveryExerciseExists()
    {
        SetupValidReferences();
        var ids = new[] { ExerciseId, Guid.NewGuid() };
        var request = new RemoveWorkoutExercisesRequest { WorkoutId = WorkoutId, ExerciseIds = ids, HardDelete = true };
        _entries.Setup(x => x.CountByIdsAsync(ids, WorkoutId, default)).ReturnsAsync(ids.Length);

        await _service.RemoveRangeAsync(request, UserId, default);

        _entries.Verify(x => x.RemoveRangeAsync(ids, WorkoutId, true, default), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Once);
    }

    [Fact]
    public async Task RemoveRangeAsync_RejectsPartiallyMissingExerciseSet()
    {
        SetupValidReferences();
        var ids = new[] { ExerciseId, Guid.NewGuid() };
        var request = new RemoveWorkoutExercisesRequest { WorkoutId = WorkoutId, ExerciseIds = ids };
        _entries.Setup(x => x.CountByIdsAsync(ids, WorkoutId, default)).ReturnsAsync(1);
        var action = () => _service.RemoveRangeAsync(request, UserId, default);
        await action.Should().ThrowAsync<NotFoundException>();
        _entries.Verify(x => x.RemoveRangeAsync(It.IsAny<IEnumerable<Guid>>(), It.IsAny<Guid>(), It.IsAny<bool>(), default), Times.Never);
    }
}
