using FitnessTracker.Core.Dtos.Requests.Workouts;
using FitnessTracker.Core.Dtos.Responses.Workouts;

namespace FitnessTracker.Core.Services.Interfaces;

public interface IWorkoutService
{
    Task<WorkoutsResponse> GetAllAsync(CancellationToken token);

    Task<WorkoutResponse> GetByIdAsync(Guid id, CancellationToken token);

    Task AddAsync(AddWorkoutRequest request, Guid userId, CancellationToken token);

    Task EditAsync(EditWorkoutRequest request, Guid userId, CancellationToken token);

    Task AdminEditAsync(EditWorkoutRequest request, CancellationToken token);

    Task RemoveRangeAsync(RemoveWorkoutsRequest request, Guid userId, CancellationToken token);
    
    Task AdminRemoveRangeAsync(RemoveWorkoutsRequest request, CancellationToken token);
}
