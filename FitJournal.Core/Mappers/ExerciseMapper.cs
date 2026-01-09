using AutoMapper;
using FitJournal.Core.Dtos.Requests.Exercises;
using FitJournal.Core.Dtos.Responses.Exercises;
using FitJournal.Domain.Entities;

namespace FitJournal.Core.Mappers;

public class ExerciseMapper : Profile
{
    public ExerciseMapper()
    {
        CreateMap<AddExerciseRequest, Exercise>();
        CreateMap<EditExerciseRequest, Exercise>();

        CreateMap<Exercise, ShortExerciseResponse>();
        CreateMap<Exercise, ExerciseResponse>();
    }
}
