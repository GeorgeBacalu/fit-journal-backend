using AutoMapper;
using FitnessTracker.Core.Dtos.Requests.Workouts;
using FitnessTracker.Core.Dtos.Responses.Workouts;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Constants;
using FitnessTracker.Infra.Exceptions;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.Core.Services;

public class WorkoutService(IUnitOfWork unitOfWork, IMapper mapper) : IWorkoutService
{
    public async Task<WorkoutsResponse> GetAllAsync(CancellationToken token = default)
    {
        var workouts = await unitOfWork.WorkoutRepository.GetAllAsync(token);

        return new()
        {
            Workouts = mapper.Map<IEnumerable<ShortWorkoutResponse>>(workouts)
        };
    }

    public async Task<WorkoutResponse> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var workout = await unitOfWork.WorkoutRepository.GetByIdAsync(id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.WorkoutIdNotFound, id));

        return mapper.Map<WorkoutResponse>(workout);
    }

    public async Task AddAsync(AddWorkoutRequest request, CancellationToken token = default)
    {
        var workouts = await unitOfWork.WorkoutRepository.GetAllAsync(token);

        if (workouts.Any(workout =>
            workout.StartedAt.Date == request.StartedAt.Date &&
            workout.StartedAt.Hour == request.StartedAt.Hour &&
            workout.StartedAt.Minute == request.StartedAt.Minute))
            throw new BadRequestException(ErrorMessages.DuplicateWorkoutStartTime);

        var workout = mapper.Map<Workout>(request);

        await unitOfWork.WorkoutRepository.AddAsync(workout, default);
        await unitOfWork.CommitAsync(token);
    }

    public async Task EditAsync(EditWorkoutRequest request, CancellationToken token = default)
    {
        var workout = await unitOfWork.WorkoutRepository.GetByIdAsync(request.Id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.WorkoutIdNotFound, request.Id));

        workout.Name = workout.Name;
        workout.Description = workout.Description;
        workout.Notes = workout.Notes;
        workout.DurationMinutes = workout.DurationMinutes;
        workout.StartedAt = workout.StartedAt;

        await unitOfWork.CommitAsync(token);
    }

    public async Task RemoveRangeAsync(RemoveWorkoutsRequest request, CancellationToken token = default)
    {
        var workouts = await unitOfWork.WorkoutRepository.GetAllByIdsAsync(request.Ids, token);

        await unitOfWork.WorkoutRepository.RemoveRangeAsync(workouts, request.IsHardDelete, token);
        await unitOfWork.CommitAsync(token);
    }
}
