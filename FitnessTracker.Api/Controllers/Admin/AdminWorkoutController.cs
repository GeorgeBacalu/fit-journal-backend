using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.Workouts;
using FitnessTracker.Core.Dtos.Responses.Workouts;
using FitnessTracker.Core.Interfaces.Services;
using FitnessTracker.Domain.Enums.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers.Admin;

[Authorize(Roles = nameof(Role.Admin))]
[Route("api/v{version:apiVersion}/[controller]")]
public class AdminWorkoutController(IWorkoutService workoutService) : BaseController
{
    private readonly IWorkoutService _workoutService = workoutService;

    /// <summary>Get all workouts</summary>
    /// <param name="request">Workout pagination info</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>All workouts</returns>
    [HttpPost("all")]
    [ProducesResponseType(typeof(UsersWorkoutsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UsersWorkoutsResponse>> GetAllAsync(WorkoutPaginationRequest request, CancellationToken token = default) =>
        Ok(await _workoutService.GetAllAsync(request, null, token));

    /// <summary>Get workout by ID</summary>
    /// <param name="id">Given workout ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Workout with given ID</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(WorkoutResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await _workoutService.GetByIdAsync(id, null, token));

    /// <summary>Add new workout</summary>
    /// <param name="request">Added workout details</param>
    /// <param name="userId">Workout's user ID</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost("{userId:guid}")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> AddAsync(AddWorkoutRequest request, Guid userId, CancellationToken token = default)
    {
        await _workoutService.AddAsync(request, userId, token);

        return Created(string.Empty, new MessageResponse(SuccessMessages.Workouts.Added));
    }

    /// <summary>Edit existing workout</summary>
    /// <param name="request">Edited workout details</param>
    /// <param name="userId">Workout's user ID</param>
    /// <param name="token">Cancellation token</param>
    [HttpPut("{userId:guid}")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> EditAsync(EditWorkoutRequest request, Guid userId, CancellationToken token = default)
    {
        await _workoutService.EditAsync(request, userId, token);

        return Ok(new MessageResponse(SuccessMessages.Workouts.Edited));
    }

    /// <summary>Remove existing workouts</summary>
    /// <param name="request">Removed workout IDs</param>
    /// <param name="userId">Workout's user ID</param>
    /// <param name="token">Cancellation token</param>
    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> RemoveRangeAsync(RemoveWorkoutsRequest request, Guid userId, CancellationToken token = default)
    {
        await _workoutService.RemoveRangeAsync(request, userId, token);

        return Ok(new MessageResponse(SuccessMessages.Workouts.RemovedRange));
    }
}
