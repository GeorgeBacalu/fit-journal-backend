using FitnessTracker.Core.Dtos.Requests.Workouts;
using FitnessTracker.Core.Interfaces.Services;
using FitnessTracker.Domain.Enums;
using FitnessTracker.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Core.Interfaces.Services.Admin;

namespace FitnessTracker.Api.Controllers.Admin;

[Authorize(Roles = nameof(Role.Admin))]
[Route("api/[controller]")]
public class AdminWorkoutController(IAdminWorkoutService adminWorkoutService) : BaseController
{
    private readonly IAdminWorkoutService _adminWorkoutService = adminWorkoutService;

    /// <summary>Add new workout and assign it to user (admin)</summary>
    /// <param name="request">Added workout details</param>
    /// <param name="userId">User ID to assign the workout to</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost("{userId:guid}")]
    public async Task<ActionResult<object>> AddAsync(AddWorkoutRequest request, Guid userId, CancellationToken token = default)
    {
        await _adminWorkoutService.AddAsync(request, userId, token);

        return Created(string.Empty, new { Message = SuccessMessages.Workouts.Added });
    }

    /// <summary>Edit existing workout (admin)</summary>
    /// <param name="request">Edited workout details</param>
    /// <param name="userId">User ID to assign the workout to</param>
    /// <param name="token">Cancellation token</param>
    [HttpPut("{userId:guid}")]
    public async Task<ActionResult<object>> AdminEditAsync(EditWorkoutRequest request, Guid userId, CancellationToken token = default)
    {
        await _adminWorkoutService.EditAsync(request, userId, token);

        return Ok(new { Message = SuccessMessages.Workouts.Edited });
    }

    /// <summary>Remove existing workouts</summary>
    /// <param name="request">Removed workouts IDs</param>
    /// <param name="userId">User ID to assign the workout to</param>
    /// <param name="token">Cancellation token</param>
    [HttpDelete("{userId:guid}")]
    public async Task<ActionResult<object>> RemoveRangeAsync(RemoveWorkoutsRequest request, Guid userId, CancellationToken token = default)
    {
        await _adminWorkoutService.RemoveRangeAsync(request, userId, token);

        return Ok(new { Message = SuccessMessages.Workouts.RemovedRange });
    }
}
