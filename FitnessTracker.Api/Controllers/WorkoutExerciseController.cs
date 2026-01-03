using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.WorkoutExercises;
using FitnessTracker.Core.Dtos.Responses.Workouts;
using FitnessTracker.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public class WorkoutExerciseController(IWorkoutExerciseService workoutExerciseService) : BaseController
{
    private readonly IWorkoutExerciseService _workoutExerciseService = workoutExerciseService;

    /// <summary>Get all workout exercises</summary>
    /// <param name="request">Workout exercise pagination info</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>All workout exercises from workout</returns>
    [HttpPost("all")]
    public async Task<ActionResult<WorkoutsResponse>> GetAllAsync(WorkoutExercisePaginationRequest request, CancellationToken token = default) =>
        Ok(await _workoutExerciseService.GetAllAsync(request, UserId, token));

    /// <summary>Get workout exercise by ID</summary>
    /// <param name="id">Given workout exercise ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Workout exercise with given ID</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<WorkoutResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await _workoutExerciseService.GetByIdAsync(id, UserId, token));

    /// <summary>Add new workout exercise</summary>
    /// <param name="request">Added workout exercise details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost]
    public async Task<ActionResult<object>> AddAsync(AddWorkoutExerciseRequest request, CancellationToken token = default)
    {
        await _workoutExerciseService.AddAsync(request, UserId, token);

        return Created(string.Empty, new { Message = SuccessMessages.WorkoutExercises.Added });
    }

    /// <summary>Edit existing workout exercise</summary>
    /// <param name="request">Edited workout exercise details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPut]
    public async Task<ActionResult<object>> EditAsync(EditWorkoutExerciseRequest request, CancellationToken token = default)
    {
        await _workoutExerciseService.EditAsync(request, UserId, token);

        return Ok(new { Message = SuccessMessages.WorkoutExercises.Edited });
    }

    /// <summary>Remove existing workout exercises</summary>
    /// <param name="request">Removed workout exercises IDs</param>
    /// <param name="token">Cancellation token</param>
    [HttpDelete]
    public async Task<ActionResult<object>> RemoveRangeAsync(RemoveWorkoutExercisesRequest request, CancellationToken token = default)
    {
        await _workoutExerciseService.RemoveRangeAsync(request, UserId, token);

        return Ok(new { Message = SuccessMessages.WorkoutExercises.RemovedRange });
    }
}
