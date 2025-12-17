using FitnessTracker.Core.Dtos.Requests.Exercises;
using FitnessTracker.Core.Dtos.Responses.Exercises;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Infra.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Route("/api/[controller]")]
public class ExerciseController(IExerciseService exerciseService) : BaseController
{
    /// <summary>Get all exercises</summary>
    /// <param name="token">Cancellation token</param>
    /// <returns>List of all exercises</returns>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<ExercisesResponse>> GetAllAsync(CancellationToken token = default) =>
        Ok(await exerciseService.GetAllAsync(token));

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

    /// <summary>Edit existing exercise</summary>
    /// <param name="request">Edited exercise details</param>
    /// <param name="token">Cancellation token</param>
    [Authorize]
    [HttpPatch]
    public async Task<ActionResult<object>> EditAsync(EditExerciseRequest request, CancellationToken token = default)
    {
        await exerciseService.EditAsync(request, token);
        return Ok(new { Message = SuccessMessages.ExerciseEdited });
    }

    /// <summary>Delete existing exercises</summary>
    /// <param name="request">Deleted exercises IDs</param>
    /// <param name="token">Cancellation token</param>
    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<object>> RemoveRangeAsync(RemoveExercisesRequest request, CancellationToken token = default)
    {
        await exerciseService.RemoveRangeAsync(request, token);
        return Ok(new { Message = SuccessMessages.ExerciseRemoved });
    }
}
