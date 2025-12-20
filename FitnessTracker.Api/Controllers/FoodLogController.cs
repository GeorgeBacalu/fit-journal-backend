using FitnessTracker.Core.Dtos.Requests.FoodLogs;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Infra.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class FoodLogController(IFoodLogService foodLogService) : BaseController
{
    /// <summary>Add new food log</summary>
    /// <param name="request">Added food log details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost]
    public async Task<ActionResult<object>> AddAsync(AddFoodLogRequest request, CancellationToken token = default)
    {
        await foodLogService.AddAsync(request, UserId, token);

        return Created(string.Empty, new { Message = ResponseMessages.FoodLogs.Added });
    }
}
