using AutoMapper;
using FitnessTracker.Core.Dtos.Requests.FoodItems;
using FitnessTracker.Core.Dtos.Responses.FoodItems;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Core.Mappers;

public class FoodItemMapper : Profile
{
    public FoodItemMapper()
    {
        CreateMap<AddFoodItemRequest, FoodItem>();
        CreateMap<EditFoodItemRequest, Goal>();

        CreateMap<FoodItem, ShortFoodItemResponse>();
        CreateMap<FoodItem, FoodItemResponse>();
    }
}
