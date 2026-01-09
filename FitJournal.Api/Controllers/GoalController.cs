using FitJournal.Core.Constants;
using FitJournal.Core.Dtos.Requests.Goals;
using FitJournal.Core.Dtos.Responses.Goals;
using FitJournal.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitJournal.Api.Controllers;

[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public class GoalController(IGoalService goalService) : BaseController
{
    private readonly IGoalService _goalService = goalService;

    /// <summary>Get all user goals</summary>
    /// <param name="request">User goal pagination info</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>All user goals</returns>
    [HttpPost("all")]
    [ProducesResponseType(typeof(GoalsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GoalsResponse>> GetAllAsync(GoalPaginationRequest request, CancellationToken token = default) =>
        Ok(await _goalService.GetAllAsync(request, UserId, token));

    /// <summary>Get user goal by ID</summary>
    /// <param name="id">Given user goal ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>User goal with given ID</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GoalResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GoalResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await _goalService.GetByIdAsync(id, UserId, token));

    /// <summary>Add new user goal</summary>
    /// <param name="request">Added user goal details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> AddAsync(AddGoalRequest request, CancellationToken token = default)
    {
        await _goalService.AddAsync(request, UserId, token);

        return Created(string.Empty, new MessageResponse(SuccessMessages.Goals.Added));
    }

    /// <summary>Edit existing user goal</summary>
    /// <param name="request">Edited user goal details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPut]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> EditAsync(EditGoalRequest request, CancellationToken token = default)
    {
        await _goalService.EditAsync(request, UserId, token);

        return Ok(new MessageResponse(SuccessMessages.Goals.Edited));
    }

    /// <summary>Remove existing user goals</summary>
    /// <param name="request">Removed user goal IDs</param>
    /// <param name="token">Cancellation token</param>
    [HttpDelete]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> RemoveRangeAsync(RemoveGoalsRequest request, CancellationToken token = default)
    {
        await _goalService.RemoveRangeAsync(request, UserId, token);

        return Ok(new MessageResponse(SuccessMessages.Goals.RemovedRange));
    }
}
