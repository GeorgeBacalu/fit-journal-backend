using AutoMapper;
using FitnessTracker.App.Dtos.Requests.Workouts;
using FitnessTracker.App.Dtos.Responses.Workouts;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.App.Mappers;

public class WorkoutMapper : Profile
{
    public WorkoutMapper()
    {
        CreateMap<AddWorkoutRequest, Workout>();
        CreateMap<Workout, GetWorkoutResponse>();
    }
}
