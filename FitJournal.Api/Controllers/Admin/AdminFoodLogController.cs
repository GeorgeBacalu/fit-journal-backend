using FitJournal.Core.Constants;
using FitJournal.Core.Dtos.Requests.FoodLogs;
using FitJournal.Core.Dtos.Responses.FoodLogs;
using FitJournal.Core.Interfaces.Services;
using FitJournal.Domain.Enums.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitJournal.Api.Controllers.Admin;

[Authorize(Roles = nameof(Role.Admin))]
[Route("api/v{version:apiVersion}/[controller]")]
public class AdminFoodLogController(IFoodLogService foodLogService) : BaseController
{
    private readonly IFoodLogService _foodLogService = foodLogService;

    /// <summary>Get all food logs</summary>
    /// <param name="request">Food log pagination info</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>All food logs</returns>
    [HttpPost("all")]
    [ProducesResponseType(typeof(UsersFoodLogsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UsersFoodLogsResponse>> GetAllAsync(FoodLogPaginationRequest request, CancellationToken token) =>
        Ok(await _foodLogService.GetAllAsync(request, null, token));

    /// <summary>Get food log by ID</summary>
    /// <param name="id">Given food log ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Food log with given ID</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(FoodLogResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FoodLogResponse>> GetByIdAsync(Guid id, CancellationToken token) =>
        Ok(await _foodLogService.GetByIdAsync(id, null, token));

    /// <summary>Add new food log</summary>
    /// <param name="request">Added food log details</param>
    /// <param name="userId">Food log's user ID</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost("{userId:guid}")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> AddAsync(AddFoodLogRequest request, Guid userId, CancellationToken token = default)
    {
        await _foodLogService.AddAsync(request, userId, token);

        return Created(string.Empty, new MessageResponse(SuccessMessages.FoodLogs.Added));
    }

    /// <summary>Edit existing food log</summary>
    /// <param name="request">Edited food log details</param>
    /// <param name="userId">Food log's user ID</param>
    /// <param name="token">Cancellation token</param>
    [HttpPut("{userId:guid}")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> EditAsync(EditFoodLogRequest request, Guid userId, CancellationToken token = default)
    {
        await _foodLogService.EditAsync(request, userId, token);

        return Ok(new MessageResponse(SuccessMessages.FoodLogs.Edited));
    }

    /// <summary>Remove existing food logs</summary>
    /// <param name="request">Removed food log IDs</param>
    /// <param name="userId">Food log's user ID</param>
    /// <param name="token">Cancellation token</param>
    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> RemoveRangeAsync(RemoveFoodLogsRequest request, Guid userId, CancellationToken token = default)
    {
        await _foodLogService.RemoveRangeAsync(request, userId, token);

        return Ok(new MessageResponse(SuccessMessages.FoodLogs.RemovedRange));
    }
}
