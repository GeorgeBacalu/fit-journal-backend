using AutoMapper;
using AutoMapper.QueryableExtensions;
using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.WorkoutExercises;
using FitnessTracker.Core.Dtos.Responses.WorkoutExercises;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Interfaces.Services;
using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Core.Services;

public class WorkoutExerciseService(IUnitOfWork unitOfWork, IMapper mapper)
    : BusinessService(unitOfWork, mapper), IWorkoutExerciseService
{
    public async Task<IWorkoutExercisesResponse> GetAllAsync(WorkoutExercisePaginationRequest request, Guid? userId, CancellationToken token)
    {
        var workout = await _unitOfWork.Workouts.GetByIdAsync(request.WorkoutId, token)
            ?? throw new NotFoundException(BusinessErrors.Workouts.IdNotFound(request.WorkoutId));

        if (userId != null && workout.UserId != userId)
            throw new ForbiddenException(BusinessErrors.WorkoutExercises.UnauthorizedAccess);

        var totalCount = await _unitOfWork.WorkoutExercises.GetAllBaseQuery(request, userId).CountAsync(token);

        if (userId != null)
            return new WorkoutExercisesResponse
            {
                TotalCount = totalCount,
                WorkoutExercises = await _unitOfWork.WorkoutExercises.GetAllQuery(request, userId)
                    .ProjectTo<ShortWorkoutExerciseResponse>(_mapper.ConfigurationProvider)
                    .ToListAsync(token)
            };

        var rows = await _unitOfWork.WorkoutExercises.GetAllQuery(request, userId)
            .Select(we => new
            {
                UserId = we.Workout != null ? we.Workout.UserId : (Guid?)null,
                UserName = we.Workout != null && we.Workout.User != null ? we.Workout.User.Name : null,
                WorkoutExercise = _mapper.Map<ShortWorkoutExerciseResponse>(we)
            }).ToListAsync(token);

        var users = rows.GroupBy(r => new { r.UserId, r.UserName })
            .Select(g => new UserWorkoutExercisesResponse
            {
                UserId = g.Key.UserId,
                UserName = g.Key.UserName,
                WorkoutExercises = [.. g.Select(x => x.WorkoutExercise)]
            }).ToList();

        return new UsersWorkoutExercisesResponse { TotalCount = totalCount, Users = users };
    }

    public async Task<WorkoutExerciseResponse> GetByIdAsync(Guid id, Guid? userId, CancellationToken token)
    {
        var workoutExercise = (userId != null
            ? await _unitOfWork.WorkoutExercises.GetByIdAsync(id, userId.Value, token)
            : await _unitOfWork.WorkoutExercises.GetByIdAsync(id, token))
            ?? throw new NotFoundException(BusinessErrors.WorkoutExercises.IdNotFound(id));

        return _mapper.Map<WorkoutExerciseResponse>(workoutExercise);
    }

    public async Task AddAsync(AddWorkoutExerciseRequest request, Guid userId, CancellationToken token)
    {
        if (!await _unitOfWork.Workouts.AnyAsync(w => w.UserId == userId && w.Id == request.WorkoutId, token))
            throw new NotFoundException(BusinessErrors.Workouts.IdNotFound(request.WorkoutId));

        if (!await _unitOfWork.Exercises.AnyAsync(e => e.Id == request.ExerciseId, token))
            throw new NotFoundException(BusinessErrors.Exercises.IdNotFound(request.ExerciseId));

        if (await _unitOfWork.WorkoutExercises.AnyAsync(we => we.WorkoutId == request.WorkoutId && we.ExerciseId == request.ExerciseId, token))
            throw new BadRequestException(ValidationErrors.WorkoutExercises.AlreadyAdded);

        var workoutExercise = _mapper.Map<WorkoutExercise>(request);

        await _unitOfWork.WorkoutExercises.AddAsync(workoutExercise, token);
        await _unitOfWork.CommitAsync(token);
    }

    public async Task EditAsync(EditWorkoutExerciseRequest request, Guid userId, CancellationToken token)
    {
        if (!await _unitOfWork.Workouts.AnyAsync(w => w.UserId == userId && w.Id == request.WorkoutId, token))
            throw new NotFoundException(BusinessErrors.Workouts.IdNotFound(request.WorkoutId));

        if (!await _unitOfWork.Exercises.AnyAsync(e => e.Id == request.ExerciseId, token))
            throw new NotFoundException(BusinessErrors.Exercises.IdNotFound(request.ExerciseId));

        var workoutExercise = await _unitOfWork.WorkoutExercises.GetByIdTrackedAsync(request.Id, token)
            ?? throw new NotFoundException(BusinessErrors.WorkoutExercises.IdNotFound(request.Id));

        if (workoutExercise.WorkoutId != request.WorkoutId || workoutExercise.ExerciseId != request.ExerciseId)
            throw new BadRequestException(ValidationErrors.WorkoutExercises.ExerciseNotInWorkout);

        _mapper.Map(request, workoutExercise);
        await _unitOfWork.CommitAsync(token);
    }

    public async Task RemoveRangeAsync(RemoveWorkoutExercisesRequest request, Guid userId, CancellationToken token)
    {
        if (!await _unitOfWork.Workouts.AnyAsync(w => w.UserId == userId && w.Id == request.WorkoutId, token))
            throw new NotFoundException(BusinessErrors.Workouts.IdNotFound(request.WorkoutId));

        if (await _unitOfWork.WorkoutExercises.CountByIdsAsync(request.ExerciseIds, request.WorkoutId, token) != request.ExerciseIds.Count())
            throw new NotFoundException(BusinessErrors.WorkoutExercises.IdsNotFound);

        await _unitOfWork.WorkoutExercises.RemoveRangeAsync(request.ExerciseIds, request.WorkoutId, request.HardDelete, token);
        await _unitOfWork.CommitAsync(token);
    }
}
