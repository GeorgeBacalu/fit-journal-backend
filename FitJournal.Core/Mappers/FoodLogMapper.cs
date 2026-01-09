using AutoMapper;
using FitJournal.Core.Dtos.Requests.FoodLogs;
using FitJournal.Core.Dtos.Responses.FoodLogs;
using FitJournal.Domain.Entities;

namespace FitJournal.Core.Mappers;

public class FoodLogMapper : Profile
{
    public FoodLogMapper()
    {
        CreateMap<AddFoodLogRequest, FoodLog>();
        CreateMap<EditFoodLogRequest, FoodLog>();

        CreateMap<FoodLog, ShortFoodLogResponse>();
        CreateMap<FoodLog, FoodLogResponse>();
    }
}
