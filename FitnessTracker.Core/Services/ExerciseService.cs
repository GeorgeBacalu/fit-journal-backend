using AutoMapper;
using AutoMapper.QueryableExtensions;
using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.Exercises;
using FitnessTracker.Core.Dtos.Responses.Exercises;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Interfaces.Services;
using FitnessTracker.Core.Interfaces.Validators;
using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Core.Services;

public class ExerciseService(IUnitOfWork unitOfWork, IMapper mapper, IExerciseValidator exerciseValidator)
    : BusinessService(unitOfWork, mapper), IExerciseService
{
    private readonly IExerciseValidator _exerciseValidator = exerciseValidator;

    public async Task<ExercisesResponse> GetAllAsync(ExercisePaginationRequest request, CancellationToken token) => new()
    {
        TotalCount = await _unitOfWork.Exercises.GetAllBaseQuery(request).CountAsync(token),
        Exercises = await _unitOfWork.Exercises.GetAllQuery(request)
            .ProjectTo<ShortExerciseResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(token)
    };

    public async Task<ExerciseResponse> GetByIdAsync(Guid id, CancellationToken token)
    {
        var exercise = await _unitOfWork.Exercises.GetByIdAsync(id, token)
            ?? throw new NotFoundException(BusinessErrors.Exercises.IdNotFound(id));

        return _mapper.Map<ExerciseResponse>(exercise);
    }

    public async Task AddAsync(AddExerciseRequest request, CancellationToken token)
    {
        await _exerciseValidator.ValidateAddAsync(request, token);

        var exercise = _mapper.Map<Exercise>(request);

        await _unitOfWork.Exercises.AddAsync(exercise, token);
        await _unitOfWork.CommitAsync(token);
    }

    public async Task EditAsync(EditExerciseRequest request, CancellationToken token)
    {
        await _exerciseValidator.ValidateEditAsync(request, token);

        var exercise = await _unitOfWork.Exercises.GetByIdTrackedAsync(request.Id, token)
            ?? throw new NotFoundException(BusinessErrors.Exercises.IdNotFound(request.Id));

        _mapper.Map(request, exercise);
        await _unitOfWork.CommitAsync(token);
    }

    public async Task RemoveRangeAsync(RemoveExercisesRequest request, CancellationToken token)
    {
        if (await _unitOfWork.WorkoutExercises.AnyInUseAsync(request.Ids, token))
            throw new BadRequestException(ValidationErrors.Exercises.AlreadyInUse);

        if (await _unitOfWork.Exercises.CountByIdsAsync(request.Ids, token) != request.Ids.Count())
            throw new NotFoundException(BusinessErrors.Exercises.IdsNotFound);

        await _unitOfWork.Exercises.RemoveRangeAsync(request.Ids, request.HardDelete, token);
        await _unitOfWork.CommitAsync(token);
    }
}
