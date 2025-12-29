using AutoMapper;
using FitnessTracker.Core.Dtos.Requests.WorkoutExercises;
using FitnessTracker.Core.Dtos.Responses.WorkoutExercises;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Core.Mappers;

public class WorkoutExerciseMapper : Profile
{
    public WorkoutExerciseMapper()
    {
        CreateMap<AddWorkoutExerciseRequest, WorkoutExercise>();
        CreateMap<EditWorkoutExerciseRequest, WorkoutExercise>()
            .ForMember(we => we.WorkoutId, opt => opt.Ignore())
            .ForMember(we => we.ExerciseId, opt => opt.Ignore());

        CreateMap<WorkoutExercise, ShortWorkoutExerciseResponse>();
        CreateMap<WorkoutExercise, WorkoutExerciseResponse>();
    }
}
