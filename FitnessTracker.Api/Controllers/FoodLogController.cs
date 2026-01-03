using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.FoodLogs;
using FitnessTracker.Core.Dtos.Responses.FoodLogs;
using FitnessTracker.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public class FoodLogController(IFoodLogService foodLogService) : BaseController
{
    private readonly IFoodLogService _foodLogService = foodLogService;

    /// <summary>Get all user food logs</summary>
    /// <param name="request">User food log pagination info</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>All user food logs</returns>
    [HttpPost("all")]
    public async Task<ActionResult<FoodLogsResponse>> GetAllAsync(FoodLogPaginationRequest request, CancellationToken token = default) =>
        Ok(await _foodLogService.GetAllAsync(request, UserId, token));

    /// <summary>Get user food log by ID</summary>
    /// <param name="id">Given user food log ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>User food log with given ID</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FoodLogResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await _foodLogService.GetByIdAsync(id, UserId, token));

    /// <summary>Add new user food log</summary>
    /// <param name="request">Added user food log details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost]
    public async Task<ActionResult<object>> AddAsync(AddFoodLogRequest request, CancellationToken token = default)
    {
        await _foodLogService.AddAsync(request, UserId, token);

        return Created(string.Empty, new { Message = SuccessMessages.FoodLogs.Added });
    }

    /// <summary>Edit existing user food log</summary>
    /// <param name="request">Edited user food log details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPut]
    public async Task<ActionResult<object>> EditAsync(EditFoodLogRequest request, CancellationToken token = default)
    {
        await _foodLogService.EditAsync(request, UserId, token);

        return Ok(new { Message = SuccessMessages.FoodLogs.Edited });
    }

    /// <summary>Remove existing user food logs</summary>
    /// <param name="request">Removed user food log IDs</param>
    /// <param name="token">Cancellation token</param>
    [HttpDelete]
    public async Task<ActionResult<object>> RemoveRangeAsync(RemoveFoodLogsRequest request, CancellationToken token = default)
    {
        await _foodLogService.RemoveRangeAsync(request, UserId, token);

        return Ok(new { Message = SuccessMessages.FoodLogs.RemovedRange });
    }
}
