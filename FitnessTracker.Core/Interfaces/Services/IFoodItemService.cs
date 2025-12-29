using FitnessTracker.Core.Dtos.Requests.FoodItems;
using FitnessTracker.Core.Dtos.Responses.FoodItems;

namespace FitnessTracker.Core.Interfaces.Services;

public interface IFoodItemService : IBusinessService
{
    Task<FoodItemsResponse> GetAllAsync(FoodItemPaginationRequest request, CancellationToken token);

    Task<FoodItemResponse> GetByIdAsync(Guid id, CancellationToken token);

    Task AddAsync(AddFoodItemRequest request, CancellationToken token);

    Task EditAsync(EditFoodItemRequest request, CancellationToken token);

    Task RemoveRangeAsync(RemoveFoodItemsRequest request, CancellationToken token);
}
