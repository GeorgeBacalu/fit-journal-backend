using FitJournal.Core.Dtos.Requests.Exercises;
using FitJournal.Core.Extensions.Pagination;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Domain.Entities;
using FitJournal.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FitJournal.Infra.Repositories;

public class ExerciseRepository(AppDbContext db)
    : BaseRepository<Exercise>(db), IExerciseRepository
{
    public IQueryable<Exercise> GetAllBaseQuery(ExercisePaginationRequest request) =>
        _db.Exercises.AsNoTracking().AddFilters(request);

    public IQueryable<Exercise> GetAllQuery(ExercisePaginationRequest request) =>
        GetAllBaseQuery(request).AddSorting(request).AddPaging(request);
}
