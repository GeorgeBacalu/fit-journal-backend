using AutoMapper;
using FitnessTracker.App.Dtos.Requests.Exercises;
using FitnessTracker.App.Dtos.Responses.Exercises;
using FitnessTracker.App.Services.Interfaces;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.App.Services;

public class ExerciseService(IUnitOfWork unitOfWork, IMapper mapper) : IExerciseService
{
    public async Task<GetExercisesResponse> GetAllAsync(CancellationToken token = default)
    {
        var exercises = await unitOfWork.ExerciseRepository.GetAllAsync(token);

        return new() { Exercises = mapper.Map<IEnumerable<GetExerciseResponse>>(exercises) };
    }

    public async Task AddAsync(AddExerciseRequest request, CancellationToken token = default)
    {
        var execise = mapper.Map<Exercise>(request);
        await unitOfWork.ExerciseRepository.AddAsync(execise, default);
        await unitOfWork.CommitAsync(default);
    }
}
