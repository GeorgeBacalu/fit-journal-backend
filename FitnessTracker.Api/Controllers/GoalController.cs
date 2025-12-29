using Asp.Versioning;
using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.Goals;
using FitnessTracker.Core.Dtos.Responses.Goals;
using FitnessTracker.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Authorize]
[ApiVersion("1.0")]
[Route("/api/v{version:apiVersion}/[controller]")]
public class GoalController(IGoalService goalService) : BaseController
{
    private readonly IGoalService _goalService = goalService;

    /// <summary>Get all user goals</summary>
    /// <param name="token">Cancellation token</param>
    /// <returns>All user goals</returns>
    [HttpGet]
    public async Task<ActionResult<GoalsResponse>> GetAllAsync(CancellationToken token = default) =>
        Ok(await _goalService.GetAllAsync(UserId, token));

    /// <summary>Get user goal by ID</summary>
    /// <param name="id">Given user goal ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>User goal with given ID</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GoalResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await _goalService.GetByIdAsync(id, UserId, token));

    /// <summary>Add new user goal</summary>
    /// <param name="request">Added user goal details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost]
    public async Task<ActionResult<object>> AddAsync(AddGoalRequest request, CancellationToken token = default)
    {
        await _goalService.AddAsync(request, UserId, token);

        return Created(string.Empty, new { Message = SuccessMessages.Goals.Added });
    }

    /// <summary>Edit existing user goal</summary>
    /// <param name="request">Edited user goal details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPut]
    public async Task<ActionResult<object>> EditAsync(EditGoalRequest request, CancellationToken token = default)
    {
        await _goalService.EditAsync(request, UserId, token);

        return Ok(new { Message = SuccessMessages.Goals.Edited });
    }

    /// <summary>Remove existing user goal</summary>
    /// <param name="request">Removed user goal IDs</param>
    /// <param name="token">Cancellation token</param>
    [HttpDelete]
    public async Task<ActionResult<object>> RemoveRangeAsync(RemoveGoalsRequest request, CancellationToken token = default)
    {
        await _goalService.RemoveRangeAsync(request, UserId, token);

        return Ok(new { Message = SuccessMessages.Goals.RemovedRange });
    }
}
