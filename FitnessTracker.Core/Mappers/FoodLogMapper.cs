using AutoMapper;
using FitnessTracker.Core.Dtos.Requests.FoodLogs;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Core.Mappers;

public class FoodLogMapper : Profile
{
    public FoodLogMapper()
    {
        CreateMap<AddFoodLogRequest, FoodLog>();
    }
}
