using AutoMapper;
using FitnessTracker.App.Dtos.Requests.Exercises;
using FitnessTracker.App.Dtos.Responses.Exercises;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.App.Mappers;

public class ExerciseMapper : Profile
{
    public ExerciseMapper()
    {
        CreateMap<AddExerciseRequest, Exercise>();
        CreateMap<Exercise, GetExerciseResponse>();
    }
}
