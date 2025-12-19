using FitnessTracker.Core.Dtos.Requests.Workouts;
using FitnessTracker.Core.Dtos.Responses.Workouts;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Infra.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Route("/api/[controller]")]
public class WorkoutController(IWorkoutService workoutService) : BaseController
{
    /// <summary>Get all workouts</summary>
    /// <param name="token">Cancellation token</param>
    /// <returns>List of all workouts</returns>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<WorkoutsResponse>> GetAllAsync(CancellationToken token = default) =>
        Ok(await workoutService.GetAllAsync(token));

    /// <summary>Get workout by ID</summary>
    /// <param name="id">Given workout ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Workout with given ID</returns>
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<WorkoutResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await workoutService.GetByIdAsync(id, token));

    /// <summary>Add new workout</summary>
    /// <param name="request">Added workout details</param>
    /// <param name="token">Cancellation token</param>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<object>> AddAsync(AddWorkoutRequest request, CancellationToken token = default)
    {
        await workoutService.AddAsync(request, token);
        return Created("", new { Message = SuccessMessages.WorkoutAdded });
    }

    /// <summary>Edit existing workout</summary>
    /// <param name="request">Edited workout details</param>
    /// <param name="token">Cancellation token</param>
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<object>> EditAsync(EditWorkoutRequest request, CancellationToken token = default)
    {
        await workoutService.EditAsync(request, token);
        return Ok(new { Message = SuccessMessages.WorkoutEdited });
    }

    /// <summary>Remove existing workouts</summary>
    /// <param name="request">Removed workouts IDs</param>
    /// <param name="token">Cancellation token</param>
    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<object>> RemoveRangeAsync(RemoveWorkoutsRequest request, CancellationToken token = default)
    {
        await workoutService.RemoveRangeAsync(request, token);
        return Ok(new { Message = SuccessMessages.WorkoutRemoved });
    }
}
