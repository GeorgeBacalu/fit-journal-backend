using AutoMapper;
using FitnessTracker.Core.Dtos.Requests.Exercises;
using FitnessTracker.Core.Dtos.Responses.Exercises;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Constants;
using FitnessTracker.Infra.Exceptions;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.Core.Services;

public class ExerciseService(IUnitOfWork unitOfWork, IMapper mapper) : IExerciseService
{
    public async Task<ExercisesResponse> GetAllAsync(CancellationToken token = default)
    {
        var exercises = await unitOfWork.ExerciseRepository.GetAllAsync(token);

        return new()
        {
            Exercises = mapper.Map<IEnumerable<ShortExerciseResponse>>(exercises)
        };
    }

    public async Task<ExerciseResponse> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var exercise = await unitOfWork.ExerciseRepository.GetByIdAsync(id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.ExerciseIdNotFound, id));

        return mapper.Map<ExerciseResponse>(exercise);
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

    public async Task RemoveRangeAsync(RemoveExercisesRequest request, CancellationToken token = default)
    {
        var exercises = await unitOfWork.ExerciseRepository.GetAllByIdsAsync(request.Ids, token);

        await unitOfWork.ExerciseRepository.RemoveRangeAsync(exercises, request.IsHardDelete, token);
        await unitOfWork.CommitAsync(token);
    }
}
