using FitnessTracker.Core.Dtos.Requests.FoodLogs;
using FitnessTracker.Core.Dtos.Requests.MeasurementLogs;
using FitnessTracker.Core.Dtos.Responses.MeasurementLogs;
using FitnessTracker.Core.Services;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Infra.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class MeasurementLogController(IMeasurementLogService measurementLogService) : BaseController
{
    /// <summary>Get all user measurement logs</summary>
    /// <param name="token">Cancellation token</param>
    /// <returns>All user measurement logs</returns>
    [HttpGet]
    public async Task<ActionResult<MeasurementLogsResponse>> GetAllAsync(CancellationToken token = default) =>
        Ok(await measurementLogService.GetAllAsync(UserId, token));

    /// <summary>Get user measurement log by ID</summary>
    /// <param name="id">Given user measurement log ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>User measurement log with given ID</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MeasurementLogResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await measurementLogService.GetByIdAsync(id, UserId, token));

    /// <summary>Add new measurement log</summary>
    /// <param name="request">Added measurement log details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost]
    public async Task<ActionResult<object>> AddAsync(AddMeasurementLogRequest request, CancellationToken token = default)
    {
        await measurementLogService.AddAsync(request, UserId, token);

        return Created(string.Empty, new { Message = ResponseMessages.MeasurementLogs.Added });
    }

    /// <summary>Edit user's measurement log</summary>
    /// <param name="request">Edited measurement log details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPut]
    public async Task<ActionResult<object>> EditAsync(EditMeasurementLogRequest request, CancellationToken token = default)
    {
        await measurementLogService.EditAsyc(request, UserId, token);

        return Ok(new { Message = ResponseMessages.MeasurementLogs.Edited });
    }

    /// <summary>Remove existing measurement log</summary>
    /// <param name="request">Removed measurement log IDs</param>
    /// <param name="token">Cancellation token</param>
    [HttpDelete]
    public async Task<ActionResult<object>> RemoveRangeAsync(RemoveMeasurementLogsRequest request, CancellationToken token = default)
    {
        await measurementLogService.RemoveRangeAsync(request, UserId, token);

        return Ok(new { Message = ResponseMessages.MeasurementLogs.RemovedRange });
    }
}
