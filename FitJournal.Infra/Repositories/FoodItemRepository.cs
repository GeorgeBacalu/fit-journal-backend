using FitJournal.Core.Dtos.Requests.FoodItems;
using FitJournal.Core.Extensions.Pagination;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Domain.Entities;
using FitJournal.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FitJournal.Infra.Repositories;

public class FoodItemRepository(AppDbContext db)
    : BaseRepository<FoodItem>(db), IFoodItemRepository
{
    public IQueryable<FoodItem> GetAllBaseQuery(FoodItemPaginationRequest request) =>
        _db.FoodItems.AsNoTracking().AddFilters(request);

    public IQueryable<FoodItem> GetAllQuery(FoodItemPaginationRequest request) =>
        GetAllBaseQuery(request).AddSorting(request).AddPaging(request);
}
