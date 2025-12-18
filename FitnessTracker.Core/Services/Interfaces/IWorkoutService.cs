using FitnessTracker.Core.Dtos.Requests.Workouts;
using FitnessTracker.Core.Dtos.Responses.Workouts;

namespace FitnessTracker.Core.Services.Interfaces;

public interface IWorkoutService
{
    Task<WorkoutsResponse> GetAllAsync(CancellationToken token = default);
    Task<WorkoutResponse> GetByIdAsync(Guid id, CancellationToken token = default);
    Task AddAsync(AddWorkoutRequest request, CancellationToken token = default);
    Task EditAsync(EditWorkoutRequest request, CancellationToken token = default);
    Task RemoveRangeAsync(RemoveWorkoutsRequest request, CancellationToken token = default);
}
