using AutoMapper;
using FitnessTracker.Core.Dtos.Requests.FoodItems;
using FitnessTracker.Core.Dtos.Responses.FoodItems;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Constants;
using FitnessTracker.Infra.Exceptions;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.Core.Services;

public class FoodItemService(IUnitOfWork unitOfWork, IMapper mapper) : IFoodItemService
{
    public async Task<FoodItemsResponse> GetAllAsync(CancellationToken token)
    {
        var foodItems = await unitOfWork.FoodItems.GetAllAsync(token);

        return new()
        {
            FoodItems = mapper.Map<IEnumerable<ShortFoodItemResponse>>(foodItems),
            TotalCount = foodItems.Count()
        };
    }

    public async Task<FoodItemResponse> GetByIdAsync(Guid id, CancellationToken token)
    {
        var foodItem = await unitOfWork.FoodItems.GetByIdAsync(id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.FoodItems.IdNotFound, id));

        return mapper.Map<FoodItemResponse>(foodItem);
    }

    public async Task AddAsync(AddFoodItemRequest request, CancellationToken token)
    {
        if (await unitOfWork.FoodItems.AnyAsync(foodItem => foodItem.Name == request.Name, token))
            throw new BadRequestException(ValidationErrors.Common.NameTaken);

        var foodItem = mapper.Map<FoodItem>(request);

        await unitOfWork.FoodItems.AddAsync(foodItem, token);
        await unitOfWork.CommitAsync(token);
    }

    public async Task EditAsync(EditFoodItemRequest request, CancellationToken token)
    {
        if (await unitOfWork.FoodItems.AnyAsync(foodItem => foodItem.Name == request.Name && foodItem.Id != request.Id, token))
            throw new BadRequestException(ValidationErrors.Common.NameTaken);

        var foodItem = await unitOfWork.FoodItems.GetByIdAsync(request.Id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.FoodItems.IdNotFound, request.Id));

        mapper.Map(request, foodItem);

        await unitOfWork.CommitAsync(token);
    }

    public async Task RemoveRangeAsync(RemoveFoodItemsRequest request, CancellationToken token)
    {
        var ids = await unitOfWork.FoodItems.GetExistingIdsAsync(request.Ids, token);

        if (ids.Count() != request.Ids.Count())
            throw new NotFoundException(ErrorMessages.FoodItems.IdsNotFound);

        await unitOfWork.FoodItems.RemoveRangeAsync(ids, request.IsHardDelete, token);
        await unitOfWork.CommitAsync(token);
    }
}
