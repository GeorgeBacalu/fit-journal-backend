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
    public async Task<ExercisesResponse> GetAllAsync(CancellationToken token)
    {
        var exercises = await unitOfWork.Exercises.GetAllAsync(token);

        return new()
        {
            Exercises = mapper.Map<IEnumerable<ShortExerciseResponse>>(exercises),
            TotalCount = exercises.Count()
        };
    }

    public async Task<ExerciseResponse> GetByIdAsync(Guid id, CancellationToken token)
    {
        var exercise = await unitOfWork.Exercises.GetByIdAsync(id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.Exercises.IdNotFound, id));

        return mapper.Map<ExerciseResponse>(exercise);
    }

    public async Task AddAsync(AddExerciseRequest request, CancellationToken token)
    {
        if (await unitOfWork.Exercises.AnyAsync(exercise => exercise.Name == request.Name, token))
            throw new BadRequestException(ValidationErrors.Exercises.NameTaken);

        var exercise = mapper.Map<Exercise>(request);

        await unitOfWork.Exercises.AddAsync(exercise, token);
        await unitOfWork.CommitAsync(token);
    }

    public async Task EditAsync(EditExerciseRequest request, CancellationToken token)
    {
        if (await unitOfWork.Exercises.AnyAsync(exercise => exercise.Name == request.Name && exercise.Id != request.Id, token))
            throw new BadRequestException(ValidationErrors.Exercises.NameTaken);

        var exercise = await unitOfWork.Exercises.GetByIdAsync(request.Id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.Exercises.IdNotFound, request.Id));

        mapper.Map(request, exercise);

        await unitOfWork.CommitAsync(token);
    }

    public async Task RemoveRangeAsync(RemoveExercisesRequest request, CancellationToken token)
    {
        var ids = await unitOfWork.Exercises.GetExistingIdsAsync(request.Ids, token);
        
        if (ids.Count() != request.Ids.Count())
            throw new NotFoundException(ErrorMessages.Exercises.IdsNotFound);

        if (await unitOfWork.Exercises.AnyInUseAsync(ids, token))
            throw new BadRequestException(ErrorMessages.Exercises.AlreadyInUse);

        await unitOfWork.Exercises.RemoveRangeAsync(ids, request.IsHardDelete, token);
        await unitOfWork.CommitAsync(token);
    }
}
