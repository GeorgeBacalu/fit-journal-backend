using FitnessTracker.Core.Dtos.Requests.Workouts;
using FitnessTracker.Core.Dtos.Responses.Workouts;

namespace FitnessTracker.Core.Interfaces.Services;

public interface IWorkoutService : IBusinessService
{
    Task<WorkoutsResponse> GetAllAsync(Guid userId, CancellationToken token);

    Task<WorkoutResponse> GetByIdAsync(Guid id, Guid userId, CancellationToken token);

    Task AddAsync(AddWorkoutRequest request, Guid userId, CancellationToken token);

    Task EditAsync(EditWorkoutRequest request, Guid userId, CancellationToken token);

    Task RemoveRangeAsync(RemoveWorkoutsRequest request, Guid userId, CancellationToken token);
}
