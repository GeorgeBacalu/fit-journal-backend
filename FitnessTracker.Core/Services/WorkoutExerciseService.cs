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
    public async Task<WorkoutExercisesResponse> GetAllAsync(Guid workoutId, Guid userId, CancellationToken token)
    {
        var workout = await _unitOfWork.Workouts.GetByIdAsync(workoutId, token)
            ?? throw new NotFoundException(string.Format(BusinessErrors.Workouts.IdNotFound, workoutId));

        if (workout.UserId != userId)
            throw new ForbiddenException(BusinessErrors.WorkoutExercises.UnauthorizedAccess);

        var workoutExercises = await _unitOfWork.WorkoutExercises.GetAllQuery(workoutId)
            .ProjectTo<ShortWorkoutExerciseResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(token);

        return new() { WorkoutExercises = workoutExercises, TotalCount = workoutExercises.Count };
    }

    public async Task<WorkoutExerciseResponse> GetByIdAsync(Guid id, Guid userId, CancellationToken token)
    {
        var workoutExercise = await _unitOfWork.WorkoutExercises.GetByIdAsync(id, userId, token)
            ?? throw new NotFoundException(string.Format(BusinessErrors.WorkoutExercises.IdNotFound, id));

        return _mapper.Map<WorkoutExerciseResponse>(workoutExercise);
    }

    public async Task AddAsync(AddWorkoutExerciseRequest request, Guid userId, CancellationToken token)
    {
        if (!await _unitOfWork.Workouts.AnyAsync(w => w.UserId == userId && w.Id == request.WorkoutId, token))
            throw new NotFoundException(string.Format(BusinessErrors.Workouts.IdNotFound, request.WorkoutId));

        if (!await _unitOfWork.Exercises.AnyAsync(e => e.Id == request.ExerciseId, token))
            throw new NotFoundException(string.Format(BusinessErrors.Exercises.IdNotFound, request.ExerciseId));

        if (await _unitOfWork.WorkoutExercises.AnyAsync(we => we.WorkoutId == request.WorkoutId && we.ExerciseId == request.ExerciseId, token))
            throw new BadRequestException(ValidationErrors.WorkoutExercises.AlreadyAdded);

        var workoutExercise = _mapper.Map<WorkoutExercise>(request);

        await _unitOfWork.WorkoutExercises.AddAsync(workoutExercise, token);
        await _unitOfWork.CommitAsync(token);
    }

    public async Task EditAsync(EditWorkoutExerciseRequest request, Guid userId, CancellationToken token)
    {
        if (!await _unitOfWork.Workouts.AnyAsync(w => w.UserId == userId && w.Id == request.WorkoutId, token))
            throw new NotFoundException(string.Format(BusinessErrors.Workouts.IdNotFound, request.WorkoutId));

        if (!await _unitOfWork.Exercises.AnyAsync(e => e.Id == request.ExerciseId, token))
            throw new NotFoundException(string.Format(BusinessErrors.Exercises.IdNotFound, request.ExerciseId));

        var workoutExercise = await _unitOfWork.WorkoutExercises.GetByIdTrackedAsync(request.Id, token)
            ?? throw new NotFoundException(string.Format(BusinessErrors.WorkoutExercises.IdNotFound, request.Id));

        if (workoutExercise.WorkoutId != request.WorkoutId || workoutExercise.ExerciseId != request.ExerciseId)
            throw new BadRequestException(ValidationErrors.WorkoutExercises.ExerciseNotInWorkout);

        _mapper.Map(request, workoutExercise);
        await _unitOfWork.CommitAsync(token);
    }

    public async Task RemoveRangeAsync(RemoveWorkoutExercisesRequest request, Guid userId, CancellationToken token)
    {
        if (!await _unitOfWork.Workouts.AnyAsync(w => w.UserId == userId && w.Id == request.WorkoutId, token))
            throw new NotFoundException(string.Format(BusinessErrors.Workouts.IdNotFound, request.WorkoutId));

        if (await _unitOfWork.WorkoutExercises.CountByIdsAsync(request.ExerciseIds, request.WorkoutId, token) != request.ExerciseIds.Count())
            throw new NotFoundException(BusinessErrors.WorkoutExercises.IdsNotFound);

        await _unitOfWork.WorkoutExercises.RemoveRangeAsync(request.ExerciseIds, request.WorkoutId, request.HardDelete, token);
        await _unitOfWork.CommitAsync(token);
    }
}
