using AutoMapper;
using FitnessTracker.Core.Dtos.Requests.Workouts;
using FitnessTracker.Core.Dtos.Responses.Workouts;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Core.Mappers;

public class WorkoutMapper : Profile
{
    public WorkoutMapper()
    {
        CreateMap<AddWorkoutRequest, Workout>();
        CreateMap<EditWorkoutRequest, Workout>()
            .ForAllMembers(options =>
                options.Condition((source, destination, sourceMember) =>
                    sourceMember != null));

        CreateMap<Workout, ShortWorkoutResponse>();
        CreateMap<Workout, WorkoutResponse>();
    }
}
