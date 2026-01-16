using AutoMapper;
using FitJournal.Core.Dtos.Common.WorkoutExercises;
using FitJournal.Core.Dtos.Requests.WorkoutExercises;
using FitJournal.Core.Dtos.Responses.WorkoutExercises;
using FitJournal.Domain.Entities;

namespace FitJournal.Core.Mappers;

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
        CreateMap<WorkoutExercise, WorkoutExerciseDto>();
    }
}
