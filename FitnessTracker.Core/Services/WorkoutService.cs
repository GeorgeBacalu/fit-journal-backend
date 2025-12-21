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
    public async Task<WorkoutsResponse> GetAllAsync(CancellationToken token)
    {
        var workouts = await unitOfWork.Workouts.GetAllAsync(token);

        return new()
        {
            Workouts = mapper.Map<IEnumerable<ShortWorkoutResponse>>(workouts),
            TotalCount = workouts.Count()
        };
    }

    public async Task<WorkoutResponse> GetByIdAsync(Guid id, CancellationToken token)
    {
        var workout = await unitOfWork.Workouts.GetByIdAsync(id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.Workouts.IdNotFound, id));

        return mapper.Map<WorkoutResponse>(workout);
    }

    public async Task AddAsync(AddWorkoutRequest request, Guid userId, CancellationToken token)
    {
        var user = await unitOfWork.Users.GetByIdAsync(userId, token)
        ?? throw new NotFoundException(string.Format(ErrorMessages.Users.IdNotFound, userId));

        if (request.StartedAt < user.CreatedAt)
            throw new BadRequestException(ErrorMessages.Workouts.BeforeRegistration);

        if (await unitOfWork.Workouts.AnyAsync(workout =>
            workout.Name == request.Name &&
            workout.UserId == userId, token))
            throw new BadRequestException(ValidationErrors.Common.NameTaken);

        if (await unitOfWork.Workouts.AnyAsync(workout =>
            workout.StartedAt.Date == request.StartedAt.Date &&
            workout.StartedAt.Hour == request.StartedAt.Hour &&
            workout.StartedAt.Minute == request.StartedAt.Minute &&
            workout.UserId == userId, token))
            throw new BadRequestException(ValidationErrors.Workouts.DuplicatedStartTime);

        var workout = mapper.Map<Workout>(request);
        workout.UserId = userId;

        await unitOfWork.Workouts.AddAsync(workout, token);
        await unitOfWork.CommitAsync(token);
    }

    public async Task EditAsync(EditWorkoutRequest request, Guid userId, CancellationToken token)
    {
        if (await unitOfWork.Workouts.AnyAsync(workout =>
            workout.Name == request.Name &&
            workout.UserId == userId &&
            workout.Id != request.Id, token))
            throw new BadRequestException(ValidationErrors.Common.NameTaken);

        if (await unitOfWork.Workouts.AnyAsync(workout =>
            workout.StartedAt.Date == request.StartedAt.Date &&
            workout.StartedAt.Hour == request.StartedAt.Hour &&
            workout.StartedAt.Minute == request.StartedAt.Minute &&
            workout.UserId == userId &&
            workout.Id != request.Id, token))
            throw new BadRequestException(ValidationErrors.Workouts.DuplicatedStartTime);

        var workout = await unitOfWork.Workouts.GetByIdAsync(request.Id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.Workouts.IdNotFound, request.Id));

        if (workout.UserId != userId)
            throw new ForbiddenException(ErrorMessages.Workouts.UnauthorizedEdit);

        mapper.Map(request, workout);

        await unitOfWork.CommitAsync(token);
    }

    public async Task AdminEditAsync(EditWorkoutRequest request, CancellationToken token)
    {
        if (await unitOfWork.Workouts.AnyAsync(workout =>
            workout.Name == request.Name &&
            workout.Id != request.Id, token))
            throw new BadRequestException(ValidationErrors.Common.NameTaken);

        if (await unitOfWork.Workouts.AnyAsync(workout =>
            workout.StartedAt.Date == request.StartedAt.Date &&
            workout.StartedAt.Hour == request.StartedAt.Hour &&
            workout.StartedAt.Minute == request.StartedAt.Minute &&
            workout.Id != request.Id, token))
            throw new BadRequestException(ValidationErrors.Workouts.DuplicatedStartTime);

        var workout = await unitOfWork.Workouts.GetByIdAsync(request.Id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.Workouts.IdNotFound, request.Id));

        mapper.Map(request, workout);

        await unitOfWork.CommitAsync(token);
    }

    public async Task RemoveRangeAsync(RemoveWorkoutsRequest request, Guid userId, CancellationToken token)
    {
        var count = await unitOfWork.Workouts.CountByIdsAsync(request.Ids, userId, token);

        if (count != request.Ids.Count())
            throw new NotFoundException(ErrorMessages.Workouts.IdsNotFound);

        await unitOfWork.Workouts.RemoveRangeAsync(request.Ids, userId, request.HardDelete, token);
        await unitOfWork.CommitAsync(token);
    }

    public Task AdminRemoveRangeAsync(RemoveWorkoutsRequest request, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
