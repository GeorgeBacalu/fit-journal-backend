using AutoMapper;
using FitJournal.Core.Dtos.Requests.ProgressLogs;
using FitJournal.Core.Dtos.Responses.ProgressLogs;
using FitJournal.Core.Exceptions;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Core.Interfaces.Validators;
using FitJournal.Core.Services;
using FitJournal.Domain.Entities;
using FluentAssertions;
using Moq;

namespace FitJournal.Test.Unit.Services;

public class ProgressLogServiceTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IProgressLogRepository> _repository = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IProgressLogValidator> _validator = new();
    private readonly ProgressLogService _service;
    private static readonly Guid UserId = Guid.NewGuid();
    private static readonly Guid LogId = Guid.NewGuid();
    private static readonly AddProgressLogRequest AddRequest = new()
    {
        Date = DateOnly.FromDateTime(DateTime.UtcNow), Weight = 80, BodyFat = 18,
        WaistCm = 85, ChestCm = 100, ArmsCm = 35
    };
    private static readonly EditProgressLogRequest EditRequest = new()
    {
        Id = LogId, Date = DateOnly.FromDateTime(DateTime.UtcNow), Weight = 79, BodyFat = 17,
        WaistCm = 84, ChestCm = 100, ArmsCm = 35
    };

    public ProgressLogServiceTest()
    {
        _unitOfWork.Setup(x => x.ProgressLogs).Returns(_repository.Object);
        _service = new ProgressLogService(_unitOfWork.Object, _mapper.Object, _validator.Object);
    }

    [Fact]
    public async Task GetByIdAsync_UsesOwnedLookupAndReturnsMappedLog()
    {
        var entity = new ProgressLog { Id = LogId, UserId = UserId };
        var response = new ProgressLogResponse { Id = LogId };
        _repository.Setup(x => x.GetByIdAsync(LogId, UserId, default)).ReturnsAsync(entity);
        _mapper.Setup(x => x.Map<ProgressLogResponse>(entity)).Returns(response);

        var result = await _service.GetByIdAsync(LogId, UserId, default);

        result.Should().BeSameAs(response);
        _repository.Verify(x => x.GetByIdAsync(LogId, UserId, default), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_UsesUnscopedLookupForAdmin()
    {
        var entity = new ProgressLog { Id = LogId };
        _repository.Setup(x => x.GetByIdAsync(LogId, default)).ReturnsAsync(entity);
        _mapper.Setup(x => x.Map<ProgressLogResponse>(entity)).Returns(new ProgressLogResponse { Id = LogId });

        await _service.GetByIdAsync(LogId, null, default);

        _repository.Verify(x => x.GetByIdAsync(LogId, default), Times.Once);
        _repository.Verify(x => x.GetByIdAsync(LogId, It.IsAny<Guid>(), default), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsNotFound_WhenOwnedLogDoesNotExist()
    {
        _repository.Setup(x => x.GetByIdAsync(LogId, UserId, default)).ReturnsAsync((ProgressLog?)null);
        var action = () => _service.GetByIdAsync(LogId, UserId, default);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AddAsync_ValidatesAssignsOwnerPersistsAndCommits()
    {
        var entity = new ProgressLog();
        _mapper.Setup(x => x.Map<ProgressLog>(AddRequest)).Returns(entity);

        await _service.AddAsync(AddRequest, UserId, default);

        _validator.Verify(x => x.ValidateAddAsync(AddRequest, UserId, default), Times.Once);
        entity.UserId.Should().Be(UserId);
        _repository.Verify(x => x.AddAsync(entity, default), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Once);
    }

    [Fact]
    public async Task EditAsync_UpdatesTrackedOwnedLogAndCommits()
    {
        var entity = new ProgressLog { Id = LogId, UserId = UserId };
        _repository.Setup(x => x.GetByIdTrackedAsync(LogId, UserId, default)).ReturnsAsync(entity);

        await _service.EditAsyc(EditRequest, UserId, default);

        _validator.Verify(x => x.ValidateEditAsync(EditRequest, UserId, default), Times.Once);
        _mapper.Verify(x => x.Map(EditRequest, entity), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Once);
    }

    [Fact]
    public async Task EditAsync_DoesNotCommit_WhenOwnedLogIsMissing()
    {
        _repository.Setup(x => x.GetByIdTrackedAsync(LogId, UserId, default)).ReturnsAsync((ProgressLog?)null);
        var action = () => _service.EditAsyc(EditRequest, UserId, default);
        await action.Should().ThrowAsync<NotFoundException>();
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Never);
    }

    [Fact]
    public async Task RemoveRangeAsync_RemovesOwnedLogsAndCommits()
    {
        var ids = new[] { LogId, Guid.NewGuid() };
        var request = new RemoveProgressLogsRequest { Ids = ids, HardDelete = true };
        _repository.Setup(x => x.CountByIdsAsync(ids, UserId, default)).ReturnsAsync(ids.Length);

        await _service.RemoveRangeAsync(request, UserId, default);

        _repository.Verify(x => x.RemoveRangeAsync(ids, UserId, true, default), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Once);
    }

    [Fact]
    public async Task RemoveRangeAsync_DoesNotMutate_WhenAnyOwnedLogIsMissing()
    {
        var ids = new[] { LogId, Guid.NewGuid() };
        var request = new RemoveProgressLogsRequest { Ids = ids };
        _repository.Setup(x => x.CountByIdsAsync(ids, UserId, default)).ReturnsAsync(1);
        var action = () => _service.RemoveRangeAsync(request, UserId, default);
        await action.Should().ThrowAsync<NotFoundException>();
        _repository.Verify(x => x.RemoveRangeAsync(It.IsAny<IEnumerable<Guid>>(), It.IsAny<Guid>(), It.IsAny<bool>(), default), Times.Never);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Never);
    }
}
