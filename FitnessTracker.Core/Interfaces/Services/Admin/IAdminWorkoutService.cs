using FitnessTracker.Core.Dtos.Requests.Workouts;

namespace FitnessTracker.Core.Interfaces.Services.Admin;

public interface IAdminWorkoutService : IBusinessService
{
    Task AddAsync(AddWorkoutRequest request, Guid userId, CancellationToken token);

    Task EditAsync(EditWorkoutRequest request, Guid userId, CancellationToken token);

    Task RemoveRangeAsync(RemoveWorkoutsRequest request, Guid userId, CancellationToken token);
}
