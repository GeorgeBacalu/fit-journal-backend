using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.ProgressLogs;
using FitnessTracker.Core.Dtos.Responses.ProgressLogs;
using FitnessTracker.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class ProgressLogController(IProgressLogService progressLogService) : BaseController
{
    private readonly IProgressLogService _progressLogService = progressLogService;

    /// <summary>Get all user progress logs</summary>
    /// <param name="token">Cancellation token</param>
    /// <returns>All user progress logs</returns>
    [HttpGet]
    public async Task<ActionResult<ProgressLogsResponse>> GetAllAsync(CancellationToken token = default) =>
        Ok(await _progressLogService.GetAllAsync(UserId, token));

    /// <summary>Get user progress log by ID</summary>
    /// <param name="id">Given user progress log ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>User progress log with given ID</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProgressLogResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await _progressLogService.GetByIdAsync(id, UserId, token));

    /// <summary>Add new user progress log</summary>
    /// <param name="request">Added user progress log details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost]
    public async Task<ActionResult<object>> AddAsync(AddProgressLogRequest request, CancellationToken token = default)
    {
        await _progressLogService.AddAsync(request, UserId, token);

        return Created(string.Empty, new { Message = SuccessMessages.ProgressLogs.Added });
    }

    /// <summary>Edit existing user progress log</summary>
    /// <param name="request">Edited user progress log details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPut]
    public async Task<ActionResult<object>> EditAsync(EditProgressLogRequest request, CancellationToken token = default)
    {
        await _progressLogService.EditAsyc(request, UserId, token);

        return Ok(new { Message = SuccessMessages.ProgressLogs.Edited });
    }

    /// <summary>Remove existing user progress log</summary>
    /// <param name="request">Removed user progress log IDs</param>
    /// <param name="token">Cancellation token</param>
    [HttpDelete]
    public async Task<ActionResult<object>> RemoveRangeAsync(RemoveProgressLogsRequest request, CancellationToken token = default)
    {
        await _progressLogService.RemoveRangeAsync(request, UserId, token);

        return Ok(new { Message = SuccessMessages.ProgressLogs.RemovedRange });
    }
}
