using AutoMapper;
using FitJournal.Core.Dtos.Requests.Workouts;
using FitJournal.Core.Dtos.Responses.Workouts;
using FitJournal.Domain.Entities;

namespace FitJournal.Core.Mappers;

public class WorkoutMapper : Profile
{
    public WorkoutMapper()
    {
        CreateMap<AddWorkoutRequest, Workout>();
        CreateMap<EditWorkoutRequest, Workout>();

        CreateMap<Workout, ShortWorkoutResponse>();
        CreateMap<Workout, WorkoutResponse>();
    }
}
