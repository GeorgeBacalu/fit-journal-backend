using FitJournal.Core.Dtos.Requests.WorkoutExercises;
using FitJournal.Core.Dtos.Responses.WorkoutExercises;

namespace FitJournal.Core.Interfaces.Services;

public interface IWorkoutExerciseService
{
    Task<IWorkoutExercisesResponse> GetAllAsync(WorkoutExercisePaginationRequest request, Guid? userId, CancellationToken token);

    Task<WorkoutExerciseResponse> GetByIdAsync(Guid id, Guid? userId, CancellationToken token);

    Task AddAsync(AddWorkoutExerciseRequest request, Guid userId, CancellationToken token);

    Task EditAsync(EditWorkoutExerciseRequest request, Guid userId, CancellationToken token);

    Task RemoveRangeAsync(RemoveWorkoutExercisesRequest request, Guid userId, CancellationToken token);
}
