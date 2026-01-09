using FitnessTracker.Core.Dtos.Requests.Exercises;
using FitnessTracker.Core.Extensions.Pagination;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Infra.Repositories;

public class ExerciseRepository(AppDbContext db)
    : BaseRepository<Exercise>(db), IExerciseRepository
{
    public IQueryable<Exercise> GetAllBaseQuery(ExercisePaginationRequest request) =>
        _db.Exercises.AsNoTracking().AddFilters(request);

    public IQueryable<Exercise> GetAllQuery(ExercisePaginationRequest request) =>
        GetAllBaseQuery(request).AddSorting(request).AddPaging(request);
}
