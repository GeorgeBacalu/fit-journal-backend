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
        var workouts = await unitOfWork.Workouts.GetAllAsync(token);

        return new()
        {
            Workouts = mapper.Map<IEnumerable<ShortWorkoutResponse>>(workouts),
            TotalCount = workouts.Count()
        };
    }

    public async Task<WorkoutResponse> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var workout = await unitOfWork.Workouts.GetByIdAsync(id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.WorkoutIdNotFound, id));

        return mapper.Map<WorkoutResponse>(workout);
    }

    public async Task AddAsync(AddWorkoutRequest request, CancellationToken token = default)
    {
        if (await unitOfWork.Workouts.AnyAsync(workout => workout.Name == request.Name, token))
            throw new BadRequestException(ValidationErrors.NameTaken);

        if (await unitOfWork.Workouts.AnyAsync(workout =>
            workout.StartedAt.Date == request.StartedAt.Date &&
            workout.StartedAt.Hour == request.StartedAt.Hour &&
            workout.StartedAt.Minute == request.StartedAt.Minute, token))
            throw new BadRequestException(ErrorMessages.DuplicatedWorkoutStartTime);

        var workout = mapper.Map<Workout>(request);

        await unitOfWork.Workouts.AddAsync(workout, token);
        await unitOfWork.CommitAsync(token);
    }

    public async Task EditAsync(EditWorkoutRequest request, CancellationToken token = default)
    {
        if (await unitOfWork.Workouts.AnyAsync(workout => workout.Name == request.Name, token))
            throw new BadRequestException(ValidationErrors.NameTaken);

        if (await unitOfWork.Workouts.AnyAsync(workout =>
            workout.StartedAt.Date == request.StartedAt.Date &&
            workout.StartedAt.Hour == request.StartedAt.Hour &&
            workout.StartedAt.Minute == request.StartedAt.Minute &&
            workout.Id != request.Id, token))
            throw new BadRequestException(ErrorMessages.DuplicatedWorkoutStartTime);

        var workout = await unitOfWork.Workouts.GetByIdAsync(request.Id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.WorkoutIdNotFound, request.Id));

        mapper.Map<Workout>(request);

        await unitOfWork.CommitAsync(token);
    }

    public async Task RemoveRangeAsync(RemoveWorkoutsRequest request, CancellationToken token = default)
    {
        var ids = await unitOfWork.Workouts.GetExistingIdsAsync(request.Ids, token);
        
        if (ids.Count() != request.Ids.Count())
            throw new NotFoundException(ErrorMessages.WorkoutIdsNotFound);

        await unitOfWork.Workouts.RemoveRangeAsync(ids, request.IsHardDelete, token);
        await unitOfWork.CommitAsync(token);
    }
}
