using AutoMapper;
using FitJournal.Core.Dtos.Requests.FoodItems;
using FitJournal.Core.Dtos.Responses.FoodItems;
using FitJournal.Core.Exceptions;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Core.Interfaces.Validators;
using FitJournal.Core.Services;
using FitJournal.Domain.Entities;
using FitJournal.Domain.Enums.FoodItems;
using FluentAssertions;
using Moq;

namespace FitJournal.Test.Unit.Services;

public class FoodItemServiceTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IFoodItemRepository> _repository = new();
    private readonly Mock<IFoodLogRepository> _foodLogs = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IFoodItemValidator> _validator = new();
    private readonly FoodItemService _service;

    private static readonly Guid FoodItemId = Guid.NewGuid();
    private static readonly AddFoodItemRequest AddRequest = new()
    {
        Name = "Oats", Calories = 380, Protein = 13, Carbs = 68, Fat = 7,
        Category = FoodCategory.Grain, Brand = FoodBrand.Generic
    };
    private static readonly EditFoodItemRequest EditRequest = new()
    {
        Id = FoodItemId, Name = "Rolled oats", Calories = 375, Protein = 13,
        Carbs = 67, Fat = 7, Category = FoodCategory.Grain, Brand = FoodBrand.Generic
    };

    public FoodItemServiceTest()
    {
        _unitOfWork.Setup(x => x.FoodItems).Returns(_repository.Object);
        _unitOfWork.Setup(x => x.FoodLogs).Returns(_foodLogs.Object);
        _service = new FoodItemService(_unitOfWork.Object, _mapper.Object, _validator.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMappedFoodItem_WhenItExists()
    {
        var entity = new FoodItem { Id = FoodItemId, Name = "Oats" };
        var response = new FoodItemResponse { Id = FoodItemId, Name = "Oats" };
        _repository.Setup(x => x.GetByIdAsync(FoodItemId, default)).ReturnsAsync(entity);
        _mapper.Setup(x => x.Map<FoodItemResponse>(entity)).Returns(response);

        var result = await _service.GetByIdAsync(FoodItemId, default);

        result.Should().BeSameAs(response);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsNotFound_WhenFoodItemDoesNotExist()
    {
        _repository.Setup(x => x.GetByIdAsync(FoodItemId, default)).ReturnsAsync((FoodItem?)null);

        var action = () => _service.GetByIdAsync(FoodItemId, default);

        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AddAsync_ValidatesMapsPersistsAndCommits()
    {
        var entity = new FoodItem { Name = AddRequest.Name };
        _mapper.Setup(x => x.Map<FoodItem>(AddRequest)).Returns(entity);

        await _service.AddAsync(AddRequest, default);

        _validator.Verify(x => x.ValidateAddAsync(AddRequest, default), Times.Once);
        _repository.Verify(x => x.AddAsync(entity, default), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Once);
    }

    [Fact]
    public async Task EditAsync_UpdatesTrackedFoodItemAndCommits()
    {
        var entity = new FoodItem { Id = FoodItemId, Name = "Oats" };
        _repository.Setup(x => x.GetByIdTrackedAsync(FoodItemId, default)).ReturnsAsync(entity);

        await _service.EditAsync(EditRequest, default);

        _validator.Verify(x => x.ValidateEditAsync(EditRequest, default), Times.Once);
        _mapper.Verify(x => x.Map(EditRequest, entity), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Once);
    }

    [Fact]
    public async Task EditAsync_DoesNotCommit_WhenFoodItemDoesNotExist()
    {
        _repository.Setup(x => x.GetByIdTrackedAsync(FoodItemId, default)).ReturnsAsync((FoodItem?)null);

        var action = () => _service.EditAsync(EditRequest, default);

        await action.Should().ThrowAsync<NotFoundException>();
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Never);
    }

    [Fact]
    public async Task RemoveRangeAsync_RejectsRequest_WhenAnyFoodItemIsMissing()
    {
        var ids = new[] { FoodItemId, Guid.NewGuid() };
        var request = new RemoveFoodItemsRequest { Ids = ids };
        _repository.Setup(x => x.CountByIdsAsync(ids, default)).ReturnsAsync(1);

        var action = () => _service.RemoveRangeAsync(request, default);

        await action.Should().ThrowAsync<NotFoundException>();
        _foodLogs.Verify(x => x.RemoveRangeFoodItemsAsync(It.IsAny<IEnumerable<Guid>>(), It.IsAny<bool>(), default), Times.Never);
        _repository.Verify(x => x.RemoveRangeAsync(It.IsAny<IEnumerable<Guid>>(), It.IsAny<bool>(), default), Times.Never);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Never);
    }

    [Fact]
    public async Task RemoveRangeAsync_RemovesDependentLogsThenItemsAndCommits()
    {
        var ids = new[] { FoodItemId, Guid.NewGuid() };
        var request = new RemoveFoodItemsRequest { Ids = ids, HardDelete = true };
        _repository.Setup(x => x.CountByIdsAsync(ids, default)).ReturnsAsync(ids.Length);
        var sequence = new MockSequence();
        _foodLogs.InSequence(sequence).Setup(x => x.RemoveRangeFoodItemsAsync(ids, true, default)).ReturnsAsync(ids.Length);
        _repository.InSequence(sequence).Setup(x => x.RemoveRangeAsync(ids, true, default)).ReturnsAsync(ids.Length);
        _unitOfWork.InSequence(sequence).Setup(x => x.CommitAsync(default)).ReturnsAsync(1);

        await _service.RemoveRangeAsync(request, default);

        _foodLogs.Verify(x => x.RemoveRangeFoodItemsAsync(ids, true, default), Times.Once);
        _repository.Verify(x => x.RemoveRangeAsync(ids, true, default), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(default), Times.Once);
    }
}
