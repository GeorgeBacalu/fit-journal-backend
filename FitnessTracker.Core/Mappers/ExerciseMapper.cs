using AutoMapper;
using FitnessTracker.Core.Dtos.Requests.Exercises;
using FitnessTracker.Core.Dtos.Responses.Exercises;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Core.Mappers;

public class ExerciseMapper : Profile
{
    public ExerciseMapper()
    {
        CreateMap<AddExerciseRequest, Exercise>();
        CreateMap<EditExerciseRequest, Exercise>()
            .ForAllMembers(options =>
                options.Condition((source, destination, sourceMember) =>
                    sourceMember != null));

        CreateMap<Exercise, ShortExerciseResponse>();
        CreateMap<Exercise, ExerciseResponse>();
    }
}
