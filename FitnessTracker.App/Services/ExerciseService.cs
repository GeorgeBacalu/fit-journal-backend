using AutoMapper;
using FitnessTracker.App.Dtos.Requests.Exercises;
using FitnessTracker.App.Dtos.Responses.Exercises;
using FitnessTracker.App.Services.Interfaces;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Constants;
using FitnessTracker.Infra.Exceptions;
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
        await unitOfWork.CommitAsync(token);
    }

    public async Task EditAsync(EditExerciseRequest request, CancellationToken token = default)
    {
        var exercise = await unitOfWork.ExerciseRepository.GetByIdAsync(request.Id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.ExerciseIdNotFound, request.Id));

        exercise.Name = request.Name ?? exercise.Name;
        exercise.Description = request.Description ?? exercise.Description;
        exercise.Notes = request.Notes ?? exercise.Notes;
        exercise.MuscleGroup = request.MuscleGroup ?? exercise.MuscleGroup;
        exercise.DifficultyLevel = request.DifficultyLevel ?? exercise.DifficultyLevel;

        await unitOfWork.CommitAsync(token);
    }
}
