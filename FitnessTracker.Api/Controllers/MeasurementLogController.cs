using FitnessTracker.Core.Dtos.Requests.MeasurementLogs;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Infra.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class MeasurementLogController(IMeasurementLogService measurementLogService) : BaseController
{
    /// <summary>Add new measurement log</summary>
    /// <param name="request">Added measurement log details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost]
    public async Task<ActionResult<object>> AddAsync(AddMeasurementLogRequest request, CancellationToken token = default)
    {
        await measurementLogService.AddAsync(request, UserId, token);

        return Created(string.Empty, new { Message = ResponseMessages.MeasurementLogs.Added });
    }
}
