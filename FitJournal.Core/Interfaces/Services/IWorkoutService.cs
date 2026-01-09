using FitJournal.Core.Dtos.Requests.Workouts;
using FitJournal.Core.Dtos.Responses.Workouts;

namespace FitJournal.Core.Interfaces.Services;

public interface IWorkoutService : IBusinessService
{
    Task<IWorkoutsResponse> GetAllAsync(WorkoutPaginationRequest request, Guid? userId, CancellationToken token);

    Task<WorkoutResponse> GetByIdAsync(Guid id, Guid? userId, CancellationToken token);

    Task AddAsync(AddWorkoutRequest request, Guid userId, CancellationToken token);

    Task EditAsync(EditWorkoutRequest request, Guid userId, CancellationToken token);

    Task RemoveRangeAsync(RemoveWorkoutsRequest request, Guid userId, CancellationToken token);
}
