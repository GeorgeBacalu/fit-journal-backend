using FitnessTracker.App.Dtos.Requests.Workouts;

namespace FitnessTracker.App.Services.Interfaces;

public interface IWorkoutService
{
    Task AddAsync(AddWorkoutRequest request, CancellationToken token = default);
}
