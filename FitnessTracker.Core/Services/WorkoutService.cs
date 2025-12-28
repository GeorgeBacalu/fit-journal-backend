using AutoMapper;
using AutoMapper.QueryableExtensions;
using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.Workouts;
using FitnessTracker.Core.Dtos.Responses.Workouts;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Interfaces.Services;
using FitnessTracker.Core.Interfaces.Validators;
using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Core.Services;

public class WorkoutService(IUnitOfWork unitOfWork, IMapper mapper, IWorkoutValidator workoutValidator)
    : BusinessService(unitOfWork, mapper), IWorkoutService
{
    private readonly IWorkoutValidator _workoutValidator = workoutValidator;

    public async Task<WorkoutsResponse> GetAllAsync(Guid userId, CancellationToken token)
    {
        var workouts = await _unitOfWork.Workouts.GetAllQuery(userId)
            .ProjectTo<ShortWorkoutResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(token);

        return new() { Workouts = workouts, TotalCount = workouts.Count };
    }

    public async Task<WorkoutResponse> GetByIdAsync(Guid id, Guid userId, CancellationToken token)
    {
        var workout = await _unitOfWork.Workouts.GetByIdAsync(id, userId, token)
            ?? throw new NotFoundException(string.Format(BusinessErrors.Workouts.IdNotFound, id));

        return _mapper.Map<WorkoutResponse>(workout);
    }

    public async Task AddAsync(AddWorkoutRequest request, Guid userId, CancellationToken token)
    {
        await _workoutValidator.ValidateAddAsync(request, userId, token);

        var workout = _mapper.Map<Workout>(request);
        workout.UserId = userId;

        await _unitOfWork.Workouts.AddAsync(workout, token);
        await _unitOfWork.CommitAsync(token);
    }

    public async Task EditAsync(EditWorkoutRequest request, Guid userId, CancellationToken token)
    {
        await _workoutValidator.ValidateEditAsync(request, userId, token);

        var workout = await _unitOfWork.Workouts.GetByIdTrackedAsync(request.Id, userId, token)
            ?? throw new NotFoundException(string.Format(BusinessErrors.Workouts.IdNotFound, request.Id));

        _mapper.Map(request, workout);
        await _unitOfWork.CommitAsync(token);
    }

    public async Task RemoveRangeAsync(RemoveWorkoutsRequest request, Guid userId, CancellationToken token)
    {
        if (await _unitOfWork.Workouts.CountByIdsAsync(request.Ids, userId, token) != request.Ids.Count())
            throw new NotFoundException(BusinessErrors.Workouts.IdsNotFound);

        await _unitOfWork.WorkoutExercises.RemoveRangeWorkoutsAsync(request.Ids, request.HardDelete, token);
        await _unitOfWork.Workouts.RemoveRangeAsync(request.Ids, userId, request.HardDelete, token);
        await _unitOfWork.CommitAsync(token);
    }
}
