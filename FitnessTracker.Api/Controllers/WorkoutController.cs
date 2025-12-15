using FitnessTracker.App.Dtos.Requests.Workouts;
using FitnessTracker.App.Dtos.Responses.Workouts;
using FitnessTracker.App.Services.Interfaces;
using FitnessTracker.Infra.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Route("/api/[controller]")]
public class WorkoutController(IWorkoutService workoutService) : BaseController
{
    /// <summary>Get current user's workouts</summary>
    /// <param name="token">Cancellation token</param>
    /// <returns>List current user's workouts</returns>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<GetWorkoutsResponse>> GetAllAsync(CancellationToken token = default)
        => Ok(await workoutService.GetAllAsync(UserId, token));

    /// <summary>Add new workout for a user</summary>
    /// <param name="request">Added workout details</param>
    /// <param name="token">Cancellation token</param>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<object>> AddAsync(AddWorkoutRequest request, CancellationToken token = default)
    {
        await workoutService.AddAsync(request, token);
        return Created("", new { Message = SuccessMessages.WorkoutAdded });
    }

    /// <summary>Edit existing workout</summary>
    /// <param name="request">Edited workout details</param>
    /// <param name="token">Cancellation token</param>
    [Authorize(Roles = "Admin")]
    [HttpPatch]
    public async Task<ActionResult<object>> EditAsync(EditWorkoutRequest request, CancellationToken token = default)
    {
        await workoutService.EditAsync(request, token);
        return Ok(new { Message = SuccessMessages.WorkoutEdited });
    }

    /// <summary>Delete workouts</summary>
    /// <param name="request">Workouts IDs to delete</param>
    /// <param name="token">Cancellation token</param>
    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public async Task<ActionResult<object>> RemoveAsync(DeleteWorkoutsRequest request, CancellationToken token = default)
    {
        await workoutService.RemoveRangeAsync(request, token);
        return Ok(new { Message = SuccessMessages.WorkoutRemoved });
    }
}
