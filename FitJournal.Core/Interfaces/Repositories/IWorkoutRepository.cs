using FitJournal.Core.Dtos.Requests.Workouts;
using FitJournal.Domain.Entities;

namespace FitJournal.Core.Interfaces.Repositories;

public interface IWorkoutRepository : IUserOwnedRepository<Workout>
{
    IQueryable<Workout> GetAllBaseQuery(WorkoutPaginationRequest request, Guid? userId);

    IQueryable<Workout> GetAllQuery(WorkoutPaginationRequest request, Guid? userId);

    Task<Workout?> GetByIdAsync(Guid id, Guid? userId, CancellationToken token);
}
