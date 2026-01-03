using AutoMapper;
using AutoMapper.QueryableExtensions;
using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.Workouts;
using FitnessTracker.Core.Dtos.Responses.Workouts;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Extensions.Pagination;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Interfaces.Services.Admin;
using FitnessTracker.Core.Interfaces.Validators;
using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Core.Services.Admin;

public class AdminWorkoutService(IUnitOfWork unitOfWork, IMapper mapper, IWorkoutValidator workoutValidator)
    : BusinessService(unitOfWork, mapper), IAdminWorkoutService
{
    private readonly IWorkoutValidator _workoutValidator = workoutValidator;

    public async Task<WorkoutsResponse> GetAllAsync(WorkoutPaginationRequest request, CancellationToken token)
    {
        var baseQuery = _unitOfWork.Workouts.GetAllQuery().AddFilters(request);

        var workouts = await baseQuery
            .AddSorting(request)
            .AddPaging(request)
            .ProjectTo<WorkoutResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(token);
        var totalCount = await baseQuery.CountAsync(token);

        return new() { Workouts = workouts, TotalCount = totalCount };
    }

    public async Task<WorkoutResponse> GetByIdAsync(Guid id, CancellationToken token)
    {
        var workout = await _unitOfWork.Workouts.GetByIdAsync(id, token)
            ?? throw new NotFoundException(BusinessErrors.Workouts.IdNotFound(id));

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
            ?? throw new NotFoundException(BusinessErrors.Workouts.IdNotFound(request.Id));

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
