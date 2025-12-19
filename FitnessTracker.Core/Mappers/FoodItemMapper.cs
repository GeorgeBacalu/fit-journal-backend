using AutoMapper;
using FitnessTracker.Core.Dtos.Requests.FoodItems;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Core.Mappers;

public class FoodItemMapper : Profile
{
    public FoodItemMapper()
    {
        CreateMap<AddFoodItemRequest, FoodItem>();
    }
}
