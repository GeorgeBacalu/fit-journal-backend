using FitnessTracker.Core.Dtos.Requests.FoodItems;

namespace FitnessTracker.Core.Services.Interfaces;

public interface IFoodItemService
{
    Task AddAsync(AddFoodItemRequest request, CancellationToken token = default);
}
