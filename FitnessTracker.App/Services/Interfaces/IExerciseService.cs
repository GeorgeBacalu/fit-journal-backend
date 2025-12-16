using FitnessTracker.App.Dtos.Requests.Exercises;
using FitnessTracker.App.Dtos.Responses.Exercises;

namespace FitnessTracker.App.Services.Interfaces;

public interface IExerciseService
{
    Task<GetExercisesResponse> GetAllAsync(CancellationToken token = default);
    Task AddAsync(AddExerciseRequest request, CancellationToken token = default);
    Task EditAsync(EditExerciseRequest request, CancellationToken token = default);
    Task RemoveRangeAsync(DeleteExercisesRequest request, CancellationToken token = default);
}
