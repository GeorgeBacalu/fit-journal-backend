using FitJournal.Core.Dtos.Requests.FoodItems;
using FitJournal.Domain.Entities;

namespace FitJournal.Core.Interfaces.Repositories;

public interface IFoodItemRepository : IBaseRepository<FoodItem>
{
    IQueryable<FoodItem> GetAllBaseQuery(FoodItemPaginationRequest request);

    IQueryable<FoodItem> GetAllQuery(FoodItemPaginationRequest request);
}
