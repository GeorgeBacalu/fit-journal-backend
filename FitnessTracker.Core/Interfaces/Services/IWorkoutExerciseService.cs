using FitnessTracker.Core.Dtos.Requests.WorkoutExercises;
using FitnessTracker.Core.Dtos.Responses.WorkoutExercises;

namespace FitnessTracker.Core.Interfaces.Services;

public interface IWorkoutExerciseService
{
    Task<WorkoutExercisesResponse> GetAllAsync(WorkoutExercisePaginationRequest request, Guid userId, CancellationToken token);

    Task<WorkoutExerciseResponse> GetByIdAsync(Guid id, Guid userId, CancellationToken token);

    Task AddAsync(AddWorkoutExerciseRequest request, Guid userId, CancellationToken token);

    Task EditAsync(EditWorkoutExerciseRequest request, Guid userId, CancellationToken token);

    Task RemoveRangeAsync(RemoveWorkoutExercisesRequest request, Guid userId, CancellationToken token);
}
