using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.Goals;
using FitnessTracker.Core.Dtos.Responses.Goals;
using FitnessTracker.Core.Interfaces.Services;
using FitnessTracker.Domain.Enums.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers.Admin;

[Authorize(Roles = nameof(Role.Admin))]
[Route("api/v{version:apiVersion}/[controller]")]
public class AdminGoalController(IGoalService goalService) : BaseController
{
    private readonly IGoalService _goalService = goalService;

    /// <summary>Get all goals</summary>
    /// <param name="request">Goal pagination info</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>All goals</returns>
    [HttpPost("all")]
    [ProducesResponseType(typeof(UsersGoalsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UsersGoalsResponse>> GetAllAsync(GoalPaginationRequest request, CancellationToken token = default) =>
        Ok(await _goalService.GetAllAsync(request, null, token));

    /// <summary>Get goal by ID</summary>
    /// <param name="id">Given goal ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Goal with given ID</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GoalResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GoalResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await _goalService.GetByIdAsync(id, null, token));

    /// <summary>Add new goal</summary>
    /// <param name="request">Added goal details</param>
    /// <param name="userId">Goal's user ID</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost("{userId:guid}")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> AddAsync(AddGoalRequest request, Guid userId, CancellationToken token = default)
    {
        await _goalService.AddAsync(request, userId, token);

        return Created(string.Empty, new MessageResponse(SuccessMessages.Goals.Added));
    }

    /// <summary>Edit existing goal</summary>
    /// <param name="request">Edited goal details</param>
    /// <param name="userId">Goal's user ID</param>
    /// <param name="token">Cancellation token</param>
    [HttpPut("{userId:guid}")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> EditAsync(EditGoalRequest request, Guid userId, CancellationToken token = default)
    {
        await _goalService.EditAsync(request, userId, token);

        return Ok(new MessageResponse(SuccessMessages.Goals.Edited));
    }

    /// <summary>Remove existing goals</summary>
    /// <param name="request">Removed goal IDs</param>
    /// <param name="userId">Goal's user ID</param>
    /// <param name="token">Cancellation token</param>
    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> RemoveRangeAsync(RemoveGoalsRequest request, Guid userId, CancellationToken token = default)
    {
        await _goalService.RemoveRangeAsync(request, userId, token);

        return Ok(new MessageResponse(SuccessMessages.Goals.RemovedRange));
    }
}
