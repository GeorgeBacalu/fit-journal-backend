using AutoMapper;
using FitJournal.Core.Dtos.Requests.Exercises;
using FitJournal.Core.Dtos.Responses.Exercises;
using FitJournal.Core.Exceptions;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Core.Interfaces.Validators;
using FitJournal.Core.Services;
using FitJournal.Domain.Entities;
using FitJournal.Domain.Enums.Exercises;
using FluentAssertions;
using Moq;

namespace FitJournal.Test.Unit.Services;

public class ExerciseServiceTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IExerciseRepository> _repository = new();
    private readonly Mock<IWorkoutExerciseRepository> _workoutExercises = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IExerciseValidator> _validator = new();
    private readonly ExerciseService _service;

    private static readonly Guid ExerciseId = Guid.NewGuid();
    private static readonly AddExerciseRequest AddRequest = new()
    {
        Name = "Squat", MuscleGroup = MuscleGroup.Legs, DifficultyLevel = DifficultyLevel.Intermediate
    };
    private static readonly EditExerciseRequest EditRequest = new()
    {
        Id = ExerciseId, Name = "Back squat", MuscleGroup = MuscleGroup.Legs,
        DifficultyLevel = DifficultyLevel.Intermediate
    };

    public ExerciseServiceTest()
    {
        _unitOfWork.Setup(x => x.Exercises).Returns(_repository.Object);
        _unitOfWork.Setup(x => x.WorkoutExercises).Returns(_workoutExercises.Object);
        _service = new ExerciseService(_unitOfWork.Object, _mapper.Object, _validator.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMappedExercise_WhenItExists()
    {
        var entity = new Exercise { Id = ExerciseId, Name = "Squat" };
        var response = new ExerciseResponse { Id = ExerciseId, Name = "Squat" };
        _repository.Setup(x => x.GetByIdAsync(ExerciseId, default)).ReturnsAsync(entity);
        _mapper.Setup(x => x.Map<ExerciseResponse>(entity)).Returns(response);

        var result = await _service.GetByIdAsync(ExerciseId, default);

        result.Should().BeSameAs(response);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsNotFound_WhenExerciseDoesNotExist()
    {
        _repository.Setup(x => x.GetByIdAsync(ExerciseId, default)).ReturnsAsync((Exercise?)null);

        var action = () => _service.GetByIdAsync(ExerciseId, default);

        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AddAsync_ValidatesMapsPersistsAndCommits()
    {
        var entity = new Exercise { Name = AddRequest.Name };
        _mapper.Setup(x => x.Map<Exercise>(AddRequest)).Returns(entity);

        await _service.AddAsync(AddRequest, default);

        _validator.Verify(x => x.ValidateAddAsync(AddRequest, default), Times.Once);
        _repository.Verify(x => x.AddAsync(entity, default), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Once);
    }

    [Fact]
    public async Task EditAsync_UpdatesTrackedExerciseAndCommits()
    {
        var entity = new Exercise { Id = ExerciseId, Name = "Squat" };
        _repository.Setup(x => x.GetByIdTrackedAsync(ExerciseId, default)).ReturnsAsync(entity);

        await _service.EditAsync(EditRequest, default);

        _validator.Verify(x => x.ValidateEditAsync(EditRequest, default), Times.Once);
        _mapper.Verify(x => x.Map(EditRequest, entity), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Once);
    }

    [Fact]
    public async Task EditAsync_DoesNotCommit_WhenExerciseDoesNotExist()
    {
        _repository.Setup(x => x.GetByIdTrackedAsync(ExerciseId, default)).ReturnsAsync((Exercise?)null);

        var action = () => _service.EditAsync(EditRequest, default);

        await action.Should().ThrowAsync<NotFoundException>();
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Never);
    }

    [Fact]
    public async Task RemoveRangeAsync_RejectsExercisesUsedByWorkouts()
    {
        var ids = new[] { ExerciseId };
        var request = new RemoveExercisesRequest { Ids = ids };
        _workoutExercises.Setup(x => x.AnyInUseAsync(ids, default)).ReturnsAsync(true);

        var action = () => _service.RemoveRangeAsync(request, default);

        await action.Should().ThrowAsync<BadRequestException>();
        _repository.Verify(x => x.RemoveRangeAsync(It.IsAny<IEnumerable<Guid>>(), It.IsAny<bool>(), default), Times.Never);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Never);
    }

    [Fact]
    public async Task RemoveRangeAsync_RejectsRequest_WhenAnyExerciseIsMissing()
    {
        var ids = new[] { ExerciseId, Guid.NewGuid() };
        var request = new RemoveExercisesRequest { Ids = ids };
        _workoutExercises.Setup(x => x.AnyInUseAsync(ids, default)).ReturnsAsync(false);
        _repository.Setup(x => x.CountByIdsAsync(ids, default)).ReturnsAsync(1);

        var action = () => _service.RemoveRangeAsync(request, default);

        await action.Should().ThrowAsync<NotFoundException>();
        _repository.Verify(x => x.RemoveRangeAsync(It.IsAny<IEnumerable<Guid>>(), It.IsAny<bool>(), default), Times.Never);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Never);
    }

    [Fact]
    public async Task RemoveRangeAsync_RemovesRequestedExercisesAndCommits()
    {
        var ids = new[] { ExerciseId, Guid.NewGuid() };
        var request = new RemoveExercisesRequest { Ids = ids, HardDelete = true };
        _workoutExercises.Setup(x => x.AnyInUseAsync(ids, default)).ReturnsAsync(false);
        _repository.Setup(x => x.CountByIdsAsync(ids, default)).ReturnsAsync(ids.Length);

        await _service.RemoveRangeAsync(request, default);

        _repository.Verify(x => x.RemoveRangeAsync(ids, true, default), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Once);
    }
}
