using FitJournal.Core.Constants;
using FitJournal.Core.Dtos.Requests.ProgressLogs;
using FitJournal.Core.Dtos.Responses.ProgressLogs;
using FitJournal.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitJournal.Api.Controllers;

[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProgressLogController(IProgressLogService progressLogService) : BaseController
{
    private readonly IProgressLogService _progressLogService = progressLogService;

    /// <summary>Get all user progress logs</summary>
    /// <param name="request">User progress log pagination info</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>All user progress logs</returns>
    [HttpPost("all")]
    [ProducesResponseType(typeof(ProgressLogsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProgressLogsResponse>> GetAllAsync(ProgressLogPaginationRequest request, CancellationToken token = default) =>
        Ok(await _progressLogService.GetAllAsync(request, UserId, token));

    /// <summary>Get user progress log by ID</summary>
    /// <param name="id">Given user progress log ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>User progress log with given ID</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProgressLogResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProgressLogResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await _progressLogService.GetByIdAsync(id, UserId, token));

    /// <summary>Add new user progress log</summary>
    /// <param name="request">Added user progress log details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> AddAsync(AddProgressLogRequest request, CancellationToken token = default)
    {
        await _progressLogService.AddAsync(request, UserId, token);

        return Created(string.Empty, new MessageResponse(SuccessMessages.ProgressLogs.Added));
    }

    /// <summary>Edit existing user progress log</summary>
    /// <param name="request">Edited user progress log details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPut]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> EditAsync(EditProgressLogRequest request, CancellationToken token = default)
    {
        await _progressLogService.EditAsyc(request, UserId, token);

        return Ok(new MessageResponse(SuccessMessages.ProgressLogs.Edited));
    }

    /// <summary>Remove existing user progress logs</summary>
    /// <param name="request">Removed user progress log IDs</param>
    /// <param name="token">Cancellation token</param>
    [HttpDelete]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> RemoveRangeAsync(RemoveProgressLogsRequest request, CancellationToken token = default)
    {
        await _progressLogService.RemoveRangeAsync(request, UserId, token);

        return Ok(new MessageResponse(SuccessMessages.ProgressLogs.RemovedRange));
    }
}
