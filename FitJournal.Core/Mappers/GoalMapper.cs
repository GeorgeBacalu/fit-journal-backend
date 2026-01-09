using AutoMapper;
using FitJournal.Core.Dtos.Requests.Goals;
using FitJournal.Core.Dtos.Responses.Goals;
using FitJournal.Domain.Entities;

namespace FitJournal.Core.Mappers;

public class GoalMapper : Profile
{
    public GoalMapper()
    {
        CreateMap<AddGoalRequest, Goal>();
        CreateMap<EditGoalRequest, Goal>();

        CreateMap<Goal, ShortGoalResponse>();
        CreateMap<Goal, GoalResponse>();
    }
}
