using AutoMapper;
using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.Workouts;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Interfaces.Services.Admin;
using FitnessTracker.Core.Interfaces.Validators;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Core.Services.Admin;

public class AdminWorkoutService(IUnitOfWork unitOfWork, IMapper mapper, IWorkoutValidator workoutValidator)
    : BusinessService(unitOfWork, mapper), IAdminWorkoutService
{
    private readonly IWorkoutValidator _workoutValidator = workoutValidator;

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
