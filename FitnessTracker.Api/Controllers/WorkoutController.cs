using FitnessTracker.Core.Dtos.Requests.Workouts;
using FitnessTracker.Core.Dtos.Responses.Workouts;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Domain.Enums;
using FitnessTracker.Infra.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Authorize]
[Route("/api/[controller]")]
public class WorkoutController(IWorkoutService workoutService) : BaseController
{
    /// <summary>Get all workouts</summary>
    /// <param name="token">Cancellation token</param>
    /// <returns>All workouts</returns>
    [HttpGet]
    public async Task<ActionResult<WorkoutsResponse>> GetAllAsync(CancellationToken token = default) =>
        Ok(await workoutService.GetAllAsync(token));

    /// <summary>Get workout by ID</summary>
    /// <param name="id">Given workout ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Workout with given ID</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<WorkoutResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await workoutService.GetByIdAsync(id, token));

    /// <summary>Add new workout</summary>
    /// <param name="request">Added workout details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost]
    public async Task<ActionResult<object>> AddAsync(AddWorkoutRequest request, CancellationToken token = default)
    {
        await workoutService.AddAsync(request, UserId, token);

        return Created(string.Empty, new { Message = ResponseMessages.Workouts.Added });
    }

    /// <summary>Add new workout and assign it to user (admin)</summary>
    /// <param name="request">Added workout details</param>
    /// <param name="userId">User ID to assign the workout to</param>
    /// <param name="token">Cancellation token</param>
    [Authorize(Roles = nameof(Role.Admin))]
    [HttpPost("admin/{userId:guid?}")]
    public async Task<ActionResult<object>> AddAsync(AddWorkoutRequest request, Guid userId, CancellationToken token = default)
    {
        await workoutService.AddAsync(request, userId, token);

        return Created(string.Empty, new { Message = ResponseMessages.Workouts.Added });
    }

    /// <summary>Edit current user's workout</summary>
    /// <param name="request">Edited workout details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPut]
    public async Task<ActionResult<object>> EditAsync(EditWorkoutRequest request, CancellationToken token = default)
    {
        await workoutService.EditAsync(request, UserId, token);

        return Ok(new { Message = ResponseMessages.Workouts.Edited });
    }

    /// <summary>Edit existing workout (admin)</summary>
    /// <param name="request">Edited workout details</param>
    /// <param name="token">Cancellation token</param>
    [Authorize(Roles = nameof(Role.Admin))]
    [HttpPost("admin")]
    public async Task<ActionResult<object>> AdminEditAsync(EditWorkoutRequest request, CancellationToken token = default)
    {
        await workoutService.AdminEditAsync(request, token);

        return Ok(new { Message = ResponseMessages.Workouts.Edited });
    }

    /// <summary>Remove existing workouts</summary>
    /// <param name="request">Removed workouts IDs</param>
    /// <param name="token">Cancellation token</param>
    [HttpDelete]
    public async Task<ActionResult<object>> RemoveRangeAsync(RemoveWorkoutsRequest request, CancellationToken token = default)
    {
        await workoutService.RemoveRangeAsync(request, UserId, token);

        return Ok(new { Message = ResponseMessages.Workouts.RemovedRange });
    }

    /// <summary>Remove existing workouts</summary>
    /// <param name="request">Removed workouts IDs</param>
    /// <param name="token">Cancellation token</param>
    [Authorize(Roles = nameof(Role.Admin))]
    [HttpDelete("admin")]
    public async Task<ActionResult<object>> AdminRemoveRangeAsync(RemoveWorkoutsRequest request, CancellationToken token = default)
    {
        await workoutService.AdminRemoveRangeAsync(request, token);

        return Ok(new { Message = ResponseMessages.Workouts.RemovedRange });
    }
}
