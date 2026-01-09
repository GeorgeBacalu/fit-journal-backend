using AutoMapper;
using FitJournal.Core.Dtos.Common.FoodItems;
using FitJournal.Core.Dtos.Requests.FoodItems;
using FitJournal.Core.Dtos.Responses.FoodItems;
using FitJournal.Domain.Entities;

namespace FitJournal.Core.Mappers;

public class FoodItemMapper : Profile
{
    public FoodItemMapper()
    {
        CreateMap<AddFoodItemRequest, FoodItem>();
        CreateMap<EditFoodItemRequest, Goal>();

        CreateMap<FoodItem, ShortFoodItemResponse>();
        CreateMap<FoodItem, FoodItemResponse>();
        CreateMap<FoodItem, FoodItemDto>();
    }
}
