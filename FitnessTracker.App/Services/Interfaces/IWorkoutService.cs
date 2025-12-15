using FitnessTracker.App.Dtos.Requests.Workouts;
using FitnessTracker.App.Dtos.Responses.Workouts;

namespace FitnessTracker.App.Services.Interfaces;

public interface IWorkoutService
{
    Task<GetWorkoutsResponse> GetAllAsync(Guid userId, CancellationToken token = default);
    Task AddAsync(AddWorkoutRequest request, CancellationToken token = default);
    Task EditAsync(EditWorkoutRequest request, CancellationToken token = default);
    Task RemoveRangeAsync(DeleteWorkoutsRequest request, CancellationToken token = default);
}
