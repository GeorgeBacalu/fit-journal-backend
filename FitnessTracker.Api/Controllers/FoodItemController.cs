using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.FoodItems;
using FitnessTracker.Core.Dtos.Responses.FoodItems;
using FitnessTracker.Core.Interfaces.Services;
using FitnessTracker.Domain.Enums.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public class FoodItemController(IFoodItemService foodItemService) : BaseController
{
    private readonly IFoodItemService _foodItemService = foodItemService;

    /// <summary>Get all food items</summary>
    /// <param name="request">Food item pagination info</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>All food items</returns>
    [HttpPost("all")]
    [ProducesResponseType(typeof(FoodItemsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<FoodItemsResponse>> GetAllAsync(FoodItemPaginationRequest request, CancellationToken token = default) =>
        Ok(await _foodItemService.GetAllAsync(request, token));

    /// <summary>Get food item by ID</summary>
    /// <param name="id">Given food item ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Food item with given ID</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(FoodItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FoodItemResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await _foodItemService.GetByIdAsync(id, token));

    /// <summary>Add new food item</summary>
    /// <param name="request">Added food item details</param>
    /// <param name="token">Cancellation token</param>
    [Authorize(Roles = nameof(Role.Admin))]
    [HttpPost]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MessageResponse>> AddAsync(AddFoodItemRequest request, CancellationToken token = default)
    {
        await _foodItemService.AddAsync(request, token);

        return Created(string.Empty, new MessageResponse(SuccessMessages.FoodItems.Added));
    }

    /// <summary>Edit existing food item</summary>
    /// <param name="request">Edited food item details</param>
    /// <param name="token">Cancellation token</param>
    [Authorize(Roles = nameof(Role.Admin))]
    [HttpPut]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> EditAsync(EditFoodItemRequest request, CancellationToken token = default)
    {
        await _foodItemService.EditAsync(request, token);

        return Ok(new MessageResponse(SuccessMessages.FoodItems.Edited));
    }

    /// <summary>Delete existing food items</summary>
    /// <param name="request">Deleted food items IDs</param>
    /// <param name="token">Cancellation token</param>
    [Authorize(Roles = nameof(Role.Admin))]
    [HttpDelete]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> RemoveRangeAsync(RemoveFoodItemsRequest request, CancellationToken token = default)
    {
        await _foodItemService.RemoveRangeAsync(request, token);

        return Ok(new MessageResponse(SuccessMessages.FoodItems.RemovedRange));
    }
}
