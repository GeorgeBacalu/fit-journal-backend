using AutoMapper;
using AutoMapper.QueryableExtensions;
using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.FoodItems;
using FitnessTracker.Core.Dtos.Responses.FoodItems;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Interfaces.Services;
using FitnessTracker.Core.Interfaces.Validators;
using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Core.Services;

public class FoodItemService(IUnitOfWork unitOfWork, IMapper mapper, IFoodItemValidator foodItemValidator)
    : BusinessService(unitOfWork, mapper), IFoodItemService
{
    private readonly IFoodItemValidator _foodItemValidator = foodItemValidator;

    public async Task<FoodItemsResponse> GetAllAsync(CancellationToken token)
    {
        var foodItems = await _unitOfWork.FoodItems.GetAllQuery()
            .ProjectTo<ShortFoodItemResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(token);

        return new() { FoodItems = foodItems, TotalCount = foodItems.Count };
    }

    public async Task<FoodItemResponse> GetByIdAsync(Guid id, CancellationToken token)
    {
        var foodItem = await _unitOfWork.FoodItems.GetByIdAsync(id, token)
            ?? throw new NotFoundException(BusinessErrors.FoodItems.IdNotFound(id));

        return _mapper.Map<FoodItemResponse>(foodItem);
    }

    public async Task AddAsync(AddFoodItemRequest request, CancellationToken token)
    {
        await _foodItemValidator.ValidateAddAsync(request, token);

        var foodItem = _mapper.Map<FoodItem>(request);

        await _unitOfWork.FoodItems.AddAsync(foodItem, token);
        await _unitOfWork.CommitAsync(token);
    }

    public async Task EditAsync(EditFoodItemRequest request, CancellationToken token)
    {
        await _foodItemValidator.ValidateEditAsync(request, token);

        var foodItem = await _unitOfWork.FoodItems.GetByIdTrackedAsync(request.Id, token)
            ?? throw new NotFoundException(BusinessErrors.FoodItems.IdNotFound(request.Id));

        _mapper.Map(request, foodItem);
        await _unitOfWork.CommitAsync(token);
    }

    public async Task RemoveRangeAsync(RemoveFoodItemsRequest request, CancellationToken token)
    {
        if (await _unitOfWork.FoodItems.CountByIdsAsync(request.Ids, token) != request.Ids.Count())
            throw new NotFoundException(BusinessErrors.FoodItems.IdsNotFound);

        await _unitOfWork.FoodLogs.RemoveRangeFoodItemsAsync(request.Ids, request.HardDelete, token);
        await _unitOfWork.FoodItems.RemoveRangeAsync(request.Ids, request.HardDelete, token);
        await _unitOfWork.CommitAsync(token);
    }
}
