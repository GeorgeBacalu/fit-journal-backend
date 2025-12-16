using FitnessTracker.App.Dtos.Requests.Exercises;
using FitnessTracker.App.Services.Interfaces;
using FitnessTracker.Infra.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Route("/api/[controller]")]
public class ExerciseController(IExerciseService exerciseService) : BaseController
{
    /// <summary>Add new exercise</summary>
    /// <param name="request">Added exercise details</param>
    /// <param name="token">Cancellation token</param>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<object>> AddAsync(AddExerciseRequest request, CancellationToken token = default)
    {
        await exerciseService.AddAsync(request, token);
        return Created("", new { Message = SuccessMessages.ExerciseAdded });
    }
}
