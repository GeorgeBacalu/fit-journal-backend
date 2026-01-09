using FitnessTracker.Core.Dtos.Requests.Exercises;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Core.Interfaces.Repositories;

public interface IExerciseRepository : IBaseRepository<Exercise>
{
    IQueryable<Exercise> GetAllBaseQuery(ExercisePaginationRequest request);

    IQueryable<Exercise> GetAllQuery(ExercisePaginationRequest request);
}
