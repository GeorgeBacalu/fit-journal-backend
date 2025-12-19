using AutoMapper;
using FitnessTracker.Core.Dtos.Requests.FoodItems;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Constants;
using FitnessTracker.Infra.Exceptions;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.Core.Services;

public class FoodItemService(IUnitOfWork unitOfWork, IMapper mapper) : IFoodItemService
{
    public async Task AddAsync(AddFoodItemRequest request, CancellationToken token = default)
    {
        if (await unitOfWork.FoodItems.AnyAsync(foodItem => foodItem.Name == request.Name, token))
            throw new BadRequestException(ValidationErrors.NameTaken);

        var foodItem = mapper.Map<FoodItem>(request);

        await unitOfWork.FoodItems.AddAsync(foodItem, token);
        await unitOfWork.CommitAsync(token);
    }
}
