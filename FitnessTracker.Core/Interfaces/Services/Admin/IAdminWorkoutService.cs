using FitnessTracker.Core.Dtos.Requests.Workouts;
using FitnessTracker.Core.Dtos.Responses.Workouts;

namespace FitnessTracker.Core.Interfaces.Services.Admin;

public interface IAdminWorkoutService : IBusinessService
{
    Task<WorkoutsResponse> GetAllAsync(WorkoutPaginationRequest request, CancellationToken token);

    Task<WorkoutResponse> GetByIdAsync(Guid id, CancellationToken token);

    Task AddAsync(AddWorkoutRequest request, Guid userId, CancellationToken token);

    Task EditAsync(EditWorkoutRequest request, Guid userId, CancellationToken token);
    
    Task RemoveRangeAsync(RemoveWorkoutsRequest request, Guid userId, CancellationToken token);
}
