using FitnessTracker.Core.Dtos.Requests.FoodItems;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Core.Interfaces.Repositories;

public interface IFoodItemRepository : IBaseRepository<FoodItem>
{
    IQueryable<FoodItem> GetAllBaseQuery(FoodItemPaginationRequest request);

    IQueryable<FoodItem> GetAllQuery(FoodItemPaginationRequest request);
}
