using FitJournal.Core.Dtos.Requests.Exercises;
using FitJournal.Domain.Entities;

namespace FitJournal.Core.Interfaces.Repositories;

public interface IExerciseRepository : IBaseRepository<Exercise>
{
    IQueryable<Exercise> GetAllBaseQuery(ExercisePaginationRequest request);

    IQueryable<Exercise> GetAllQuery(ExercisePaginationRequest request);
}
