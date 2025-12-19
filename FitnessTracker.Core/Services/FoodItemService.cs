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
    public async Task<FoodItemsResponse> GetAllAsync(CancellationToken token = default)
    {
        var foodItems = await unitOfWork.FoodItems.GetAllAsync(token);

        return new()
        {
            FoodItems = mapper.Map<IEnumerable<FoodItemResponse>>(foodItems),
            TotalCount = foodItems.Count()
        };
    }

    public async Task<FoodItemResponse> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var foodItem = await unitOfWork.FoodItems.GetByIdAsync(id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.FoodItemIdNotFound, id));

        return mapper.Map<FoodItemResponse>(foodItem);
    }

    public async Task AddAsync(AddFoodItemRequest request, CancellationToken token = default)
    {
        if (await unitOfWork.FoodItems.AnyAsync(foodItem => foodItem.Name == request.Name, token))
            throw new BadRequestException(ValidationErrors.NameTaken);

        var foodItem = mapper.Map<FoodItem>(request);

        await unitOfWork.FoodItems.AddAsync(foodItem, token);
        await unitOfWork.CommitAsync(token);
    }

    public async Task EditAsync(EditFoodItemRequest request, CancellationToken token = default)
    {
        if (await unitOfWork.FoodItems.AnyAsync(foodItem => foodItem.Name == request.Name && foodItem.Id != request.Id, token))
            throw new BadRequestException(ValidationErrors.NameTaken);

        var foodItem = await unitOfWork.FoodItems.GetByIdAsync(request.Id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.FoodItemIdNotFound, request.Id));

        mapper.Map(request, foodItem);

        await unitOfWork.CommitAsync(token);
    }
}
