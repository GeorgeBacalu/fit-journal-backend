using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.ProgressLogs;
using FitnessTracker.Core.Dtos.Responses.ProgressLogs;
using FitnessTracker.Core.Interfaces.Services;
using FitnessTracker.Domain.Enums.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers.Admin;

[Authorize(Roles = nameof(Role.Admin))]
[Route("api/v{version:apiVersion}/[controller]")]
public class AdminProgressLogController(IProgressLogService progressLogService) : BaseController
{
    private readonly IProgressLogService _progressLogService = progressLogService;

    /// <summary>Get all progress logs</summary>
    /// <param name="request">Progress log pagination info</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>All progress logs</returns>
    [HttpPost("all")]
    [ProducesResponseType(typeof(ProgressLogsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProgressLogsResponse>> GetAllAsync(ProgressLogPaginationRequest request, CancellationToken token = default) =>
        Ok(await _progressLogService.GetAllAsync(request, null, token));

    /// <summary>Get progress log by ID</summary>
    /// <param name="id">Given progress log ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Progress log with given ID</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProgressLogResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProgressLogResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await _progressLogService.GetByIdAsync(id, null, token));

    /// <summary>Add new progress log</summary>
    /// <param name="request">Added progress log details</param>
    /// <param name="userId">Progress log's user ID</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost("{userId:guid}")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> AddAsync(AddProgressLogRequest request, Guid userId, CancellationToken token = default)
    {
        await _progressLogService.AddAsync(request, userId, token);

        return Created(string.Empty, new MessageResponse(SuccessMessages.ProgressLogs.Added));
    }

    /// <summary>Edit existing progress log</summary>
    /// <param name="request">Edited progress log details</param>
    /// <param name="userId">Progress log's user ID</param>
    /// <param name="token">Cancellation token</param>
    [HttpPut("{userId:guid}")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> EditAsync(EditProgressLogRequest request, Guid userId, CancellationToken token = default)
    {
        await _progressLogService.EditAsyc(request, userId, token);

        return Ok(new MessageResponse(SuccessMessages.ProgressLogs.Edited));
    }

    /// <summary>Remove existing progress logs</summary>
    /// <param name="request">Removed user progress log IDs</param>
    /// <param name="userId">Progress log's user ID</param>
    /// <param name="token">Cancellation token</param>
    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> RemoveRangeAsync(RemoveProgressLogsRequest request, Guid userId, CancellationToken token = default)
    {
        await _progressLogService.RemoveRangeAsync(request, userId, token);

        return Ok(new MessageResponse(SuccessMessages.ProgressLogs.RemovedRange));
    }
}
