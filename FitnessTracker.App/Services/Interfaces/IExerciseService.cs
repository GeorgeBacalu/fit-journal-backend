using FitnessTracker.App.Dtos.Requests.Exercises;

namespace FitnessTracker.App.Services.Interfaces;

public interface IExerciseService
{
    Task AddAsync(AddExerciseRequest request, CancellationToken token = default);
}
