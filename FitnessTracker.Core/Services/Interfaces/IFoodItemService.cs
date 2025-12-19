using FitnessTracker.Core.Dtos.Requests.FoodItems;
using FitnessTracker.Core.Dtos.Responses.FoodItems;

namespace FitnessTracker.Core.Services.Interfaces;

public interface IFoodItemService
{
    Task<FoodItemsResponse> GetAllAsync(CancellationToken token = default);
    Task<FoodItemResponse> GetByIdAsync(Guid id, CancellationToken token = default);
    Task AddAsync(AddFoodItemRequest request, CancellationToken token = default);
}
