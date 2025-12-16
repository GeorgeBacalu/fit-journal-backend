using FitnessTracker.Core.Dtos.Requests.Exercises;
using FitnessTracker.Core.Dtos.Responses.Exercises;

namespace FitnessTracker.Core.Services.Interfaces;

public interface IExerciseService
{
    Task<GetExercisesResponse> GetAllAsync(CancellationToken token = default);
    Task AddAsync(AddExerciseRequest request, CancellationToken token = default);
    Task EditAsync(EditExerciseRequest request, CancellationToken token = default);
    Task RemoveRangeAsync(DeleteExercisesRequest request, CancellationToken token = default);
}
