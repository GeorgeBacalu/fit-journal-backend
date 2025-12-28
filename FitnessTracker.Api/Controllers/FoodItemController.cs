using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.FoodItems;
using FitnessTracker.Core.Dtos.Responses.FoodItems;
using FitnessTracker.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Authorize]
[Route("/api/[controller]")]
public class FoodItemController(IFoodItemService foodItemService) : BaseController
{
    private readonly IFoodItemService _foodItemService = foodItemService;

    /// <summary>Get all food items</summary>
    /// <param name="token">Cancellation token</param>
    /// <returns>All food items</returns>
    [HttpGet]
    public async Task<ActionResult<FoodItemsResponse>> GetAllAsync(CancellationToken token = default) =>
        Ok(await _foodItemService.GetAllAsync(token));

    /// <summary>Get food item by ID</summary>
    /// <param name="id">Given food item ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Food item with given ID</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FoodItemResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await _foodItemService.GetByIdAsync(id, token));

    /// <summary>Add new food item</summary>
    /// <param name="request">Added food item details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost]
    public async Task<ActionResult<object>> AddAsync(AddFoodItemRequest request, CancellationToken token = default)
    {
        await _foodItemService.AddAsync(request, token);

        return Created(string.Empty, new { Message = SuccessMessages.FoodItems.Added });
    }

    /// <summary>Edit existing food item</summary>
    /// <param name="request">Edited food item details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPut]
    public async Task<ActionResult<object>> EditAsync(EditFoodItemRequest request, CancellationToken token = default)
    {
        await _foodItemService.EditAsync(request, token);

        return Ok(new { Message = SuccessMessages.FoodItems.Edited });
    }

    /// <summary>Delete existing food items</summary>
    /// <param name="request">Deleted food items IDs</param>
    /// <param name="token">Cancellation token</param>
    [HttpDelete]
    public async Task<ActionResult<object>> RemoveRangeAsync(RemoveFoodItemsRequest request, CancellationToken token = default)
    {
        await _foodItemService.RemoveRangeAsync(request, token);

        return Ok(new { Message = SuccessMessages.FoodItems.RemovedRange });
    }
}
