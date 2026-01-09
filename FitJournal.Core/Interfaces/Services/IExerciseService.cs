using FitJournal.Core.Dtos.Requests.Exercises;
using FitJournal.Core.Dtos.Responses.Exercises;

namespace FitJournal.Core.Interfaces.Services;

public interface IExerciseService : IBusinessService
{
    Task<ExercisesResponse> GetAllAsync(ExercisePaginationRequest request, CancellationToken token);

    Task<ExerciseResponse> GetByIdAsync(Guid id, CancellationToken token);

    Task AddAsync(AddExerciseRequest request, CancellationToken token);

    Task EditAsync(EditExerciseRequest request, CancellationToken token);

    Task RemoveRangeAsync(RemoveExercisesRequest request, CancellationToken token);
}
