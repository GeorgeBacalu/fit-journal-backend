using FitnessTracker.Core.Dtos.Requests.FoodItems;
using FitnessTracker.Core.Extensions.Pagination;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Repositories;

public class FoodItemRepository(AppDbContext db)
    : BaseRepository<FoodItem>(db), IFoodItemRepository
{
    public IQueryable<FoodItem> GetAllBaseQuery(FoodItemPaginationRequest request) =>
        _db.FoodItems.AsNoTracking().AddFilters(request);

    public IQueryable<FoodItem> GetAllQuery(FoodItemPaginationRequest request) =>
        GetAllBaseQuery(request).AddSorting(request).AddPaging(request);
}
