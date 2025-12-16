using AutoMapper;
using FitnessTracker.App.Dtos.Requests.Workouts;
using FitnessTracker.App.Dtos.Responses.Workouts;
using FitnessTracker.App.Services.Interfaces;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Domain.Enums;
using FitnessTracker.Infra.Constants;
using FitnessTracker.Infra.Exceptions;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.App.Services;

public class WorkoutService(IUnitOfWork unitOfWork, IMapper mapper) : IWorkoutService
{
    public async Task<GetWorkoutsResponse> GetAllAsync(Guid userId, CancellationToken token = default)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(userId, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.UserIdNotFound, userId));

        var workouts = user.Role == Role.User ? user.Workouts : await unitOfWork.WorkoutRepository.GetAllAsync(token);

        return new() { Workouts = mapper.Map<IEnumerable<GetWorkoutResponse>>(workouts) };
    }

    public async Task AddAsync(AddWorkoutRequest request, CancellationToken token = default)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(request.UserId, default)
            ?? throw new NotFoundException(string.Format(ErrorMessages.UserIdNotFound, request.UserId));

        if (request.StartedAt < user.CreatedAt)
            throw new BadRequestException(ErrorMessages.WorkoutBeforeRegistration);

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

        workout.Name = request.Name ?? workout.Name;
        workout.Description = request.Description ?? workout.Description;
        workout.Notes = request.Notes ?? workout.Notes;
        workout.DurationMinutes = request.DurationMinutes ?? workout.DurationMinutes;
        workout.StartedAt = request.StartedAt ?? workout.StartedAt;

        await unitOfWork.CommitAsync(token);
    }

    public async Task RemoveRangeAsync(DeleteWorkoutsRequest request, CancellationToken token = default)
    {
        var workouts = await unitOfWork.WorkoutRepository.GetAllByIdsAsync(request.Ids, token);

        foreach (var workout in workouts)
            workout.DeletedAt = DateTime.UtcNow;

        await unitOfWork.CommitAsync(token);
    }
}
