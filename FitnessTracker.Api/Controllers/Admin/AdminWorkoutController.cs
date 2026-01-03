using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.Workouts;
using FitnessTracker.Core.Dtos.Responses.Workouts;
using FitnessTracker.Core.Interfaces.Services.Admin;
using FitnessTracker.Domain.Enums.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers.Admin;

[Authorize(Roles = nameof(Role.Admin))]
[Route("api/v{version:apiVersion}/[controller]")]
public class AdminWorkoutController(IAdminWorkoutService adminWorkoutService) : BaseController
{
    private readonly IAdminWorkoutService _adminWorkoutService = adminWorkoutService;

    /// <summary>Get all workouts</summary>
    /// <param name="request">Workout pagination info</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>All workouts</returns>
    [HttpPost("all")]
    public async Task<ActionResult<WorkoutsResponse>> GetAllAsync(WorkoutPaginationRequest request, CancellationToken token = default) =>
        Ok(await _adminWorkoutService.GetAllAsync(request, token));

    /// <summary>Get workout by ID</summary>
    /// <param name="id">Given workout ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Workout with given ID</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<WorkoutResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await _adminWorkoutService.GetByIdAsync(id, token));

    /// <summary>Add new user workout (admin)</summary>
    /// <param name="request">Added user workout details</param>
    /// <param name="userId">Workout's user ID</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost("{userId:guid}")]
    public async Task<ActionResult<object>> AddAsync(AddWorkoutRequest request, Guid userId, CancellationToken token = default)
    {
        await _adminWorkoutService.AddAsync(request, userId, token);

        return Created(string.Empty, new { Message = SuccessMessages.Workouts.Added });
    }

    /// <summary>Edit existing user workout (admin)</summary>
    /// <param name="request">Edited user workout details</param>
    /// <param name="userId">Workout's user ID</param>
    /// <param name="token">Cancellation token</param>
    [HttpPut("{userId:guid}")]
    public async Task<ActionResult<object>> EditAsync(EditWorkoutRequest request, Guid userId, CancellationToken token = default)
    {
        await _adminWorkoutService.EditAsync(request, userId, token);

        return Ok(new { Message = SuccessMessages.Workouts.Edited });
    }

    /// <summary>Remove existing user workouts</summary>
    /// <param name="request">Removed user workout IDs</param>
    /// <param name="userId">Workout's user ID</param>
    /// <param name="token">Cancellation token</param>
    [HttpDelete("{userId:guid}")]
    public async Task<ActionResult<object>> RemoveRangeAsync(RemoveWorkoutsRequest request, Guid userId, CancellationToken token = default)
    {
        await _adminWorkoutService.RemoveRangeAsync(request, userId, token);

        return Ok(new { Message = SuccessMessages.Workouts.RemovedRange });
    }
}
