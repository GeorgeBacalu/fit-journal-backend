using FitnessTracker.Core.Dtos.Requests.FoodLogs;
using FitnessTracker.Core.Dtos.Responses.FoodLogs;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Infra.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class FoodLogController(IFoodLogService foodLogService) : BaseController
{
    /// <summary>Get all user food logs</summary>
    /// <param name="token">Cancellation token</param>
    /// <returns>All user food logs</returns>
    [HttpGet]
    public async Task<ActionResult<FoodLogsResponse>> GetAllAsync(CancellationToken token = default) =>
        Ok(await foodLogService.GetAllByUserAsync(UserId, token));

    /// <summary>Get food log by ID</summary>
    /// <param name="id">Given food log ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Food log with given ID</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FoodLogResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await foodLogService.GetByIdAsync(id, token));

    /// <summary>Add new food log</summary>
    /// <param name="request">Added food log details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost]
    public async Task<ActionResult<object>> AddAsync(AddFoodLogRequest request, CancellationToken token = default)
    {
        await foodLogService.AddAsync(request, UserId, token);

        return Created(string.Empty, new { Message = ResponseMessages.FoodLogs.Added });
    }

    /// <summary>Edit current user's food log</summary>
    /// <param name="request">Edited food log details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPut]
    public async Task<ActionResult<object>> EditAsync(EditFoodLogRequest request, CancellationToken token = default)
    {
        await foodLogService.EditAsync(request, UserId, token);
        
        return Ok(new { Message = ResponseMessages.FoodLogs.Edited });
    }
}
