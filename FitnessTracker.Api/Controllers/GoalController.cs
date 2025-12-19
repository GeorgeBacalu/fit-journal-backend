using FitnessTracker.Core.Dtos.Requests.Goals;
using FitnessTracker.Core.Dtos.Requests.Workouts;
using FitnessTracker.Core.Dtos.Responses.Goals;
using FitnessTracker.Core.Services;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Infra.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Route("/api/[controller]")]
public class GoalController(IGoalService goalService) : BaseController
{
    /// <summary>Get all current user's goals by achievement status)</summary>
    /// <param name="isAchieved">Achievement status</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>List of all current user's goals by achievement status</returns>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<GoalsResponse>> GetAllByUserAsync(bool isAchieved = false, CancellationToken token = default) =>
        Ok(await goalService.GetAllByUserAsync(UserId, isAchieved, token));

    /// <summary>Add new goal for current user</summary>
    /// <param name="request">Added goal details</param>
    /// <param name="token">Cancellation token</param>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<object>> AddAsync(AddGoalRequest request, CancellationToken token = default)
    {
        await goalService.AddAsync(request, UserId, token);
        return Created("", new { Message = SuccessMessages.GoalAdded });
    }

    /// <summary>Edit current user's goal</summary>
    /// <param name="request">Edited goals details</param>
    /// <param name="token">Cancellation token</param>
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<object>> EditAsync(EditGoalRequest request, CancellationToken token = default)
    {
        await goalService.EditAsync(request, UserId, token);
        return Ok(new { Message = SuccessMessages.GoalEdited });
    }

    /// <summary>Remove existing workouts</summary>
    /// <param name="request">Removed workouts IDs</param>
    /// <param name="token">Cancellation token</param>
    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<object>> RemoveRangeAsync(RemoveGoalsRequest request, CancellationToken token = default)
    {
        await goalService.RemoveRangeAsync(request, UserId, token);
        return Ok(new { Message = SuccessMessages.GoalsRemoved });
    }
}
