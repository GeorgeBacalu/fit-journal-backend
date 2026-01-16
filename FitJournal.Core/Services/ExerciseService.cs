using AutoMapper;
using AutoMapper.QueryableExtensions;
using FitJournal.Core.Constants;
using FitJournal.Core.Dtos.Requests.Exercises;
using FitJournal.Core.Dtos.Responses.Exercises;
using FitJournal.Core.Exceptions;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Core.Interfaces.Services;
using FitJournal.Core.Interfaces.Validators;
using FitJournal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitJournal.Core.Services;

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
