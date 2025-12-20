using AutoMapper;
using FitnessTracker.Core.Dtos.Requests.Goals;
using FitnessTracker.Core.Dtos.Responses.Goals;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Core.Mappers;

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
