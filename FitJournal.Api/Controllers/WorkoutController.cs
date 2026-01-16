using FitJournal.Core.Constants;
using FitJournal.Core.Dtos.Requests.Workouts;
using FitJournal.Core.Dtos.Responses.Workouts;
using FitJournal.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitJournal.Api.Controllers;

[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public class WorkoutController(IWorkoutService workoutService) : BaseController
{
    private readonly IWorkoutService _workoutService = workoutService;

    /// <summary>Get all user workouts</summary>
    /// <param name="request">User workout pagination info</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>All user workouts</returns>
    [HttpPost("all")]
    [ProducesResponseType(typeof(WorkoutsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<WorkoutsResponse>> GetAllAsync(WorkoutPaginationRequest request, CancellationToken token = default) =>
        Ok(await _workoutService.GetAllAsync(request, UserId, token));

    /// <summary>Get user workout by ID</summary>
    /// <param name="id">Given user workout ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>User workout with given ID</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(WorkoutResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await _workoutService.GetByIdAsync(id, UserId, token));

    /// <summary>Add new user workout</summary>
    /// <param name="request">Added user workout details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> AddAsync(AddWorkoutRequest request, CancellationToken token = default)
    {
        await _workoutService.AddAsync(request, UserId, token);

        return Created(string.Empty, new MessageResponse(SuccessMessages.Workouts.Added));
    }

    /// <summary>Edit existing user workout</summary>
    /// <param name="request">Edited user workout details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPut]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> EditAsync(EditWorkoutRequest request, CancellationToken token = default)
    {
        await _workoutService.EditAsync(request, UserId, token);

        return Ok(new MessageResponse(SuccessMessages.Workouts.Edited));
    }

    /// <summary>Remove existing user workouts</summary>
    /// <param name="request">Removed user workout IDs</param>
    /// <param name="token">Cancellation token</param>
    [HttpDelete]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> RemoveRangeAsync(RemoveWorkoutsRequest request, CancellationToken token = default)
    {
        await _workoutService.RemoveRangeAsync(request, UserId, token);

        return Ok(new MessageResponse(SuccessMessages.Workouts.RemovedRange));
    }
}
