using FitnessTracker.Core.Dtos.Requests.FoodItems;
using FitnessTracker.Core.Dtos.Responses.FoodItems;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Infra.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Route("/api/[controller]")]
public class FoodItemController(IFoodItemService foodItemService) : BaseController
{
    /// <summary>Get all food items</summary>
    /// <param name="token">Cancellation token</param>
    /// <returns>List of all food items</returns>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<FoodItemsResponse>> GetAllAsync(CancellationToken token = default) =>
        Ok(await foodItemService.GetAllAsync(token));

    /// <summary>Add new food item</summary>
    /// <param name="request">Added food item details</param>
    /// <param name="token">Cancellation token</param>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<object>> AddAsync(AddFoodItemRequest request, CancellationToken token = default)
    {
        await foodItemService.AddAsync(request, token);
        return Created("", new { Message = SuccessMessages.FoodItemAdded });
    }
}
