using FitnessTracker.App.Dtos.Requests.Exercises;
using FitnessTracker.App.Dtos.Responses.Exercises;
using FitnessTracker.App.Services.Interfaces;
using FitnessTracker.Infra.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Route("/api/[controller]")]
public class ExerciseController(IExerciseService exerciseService) : BaseController
{
    /// <summary>Get all exercises</summary>
    /// <param name="token">Cancellation token</param>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<GetExercisesResponse>> GetAllAsync(CancellationToken token = default)
        => Ok(await exerciseService.GetAllAsync(token));

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

    /// <summary>Delete exercises</summary>
    /// <param name="request">Exercise IDs to delete</param>
    /// <param name="token">Cancellation token</param>
    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<object>> RemoveAsync(DeleteExercisesRequest request, CancellationToken token = default)
    {
        await exerciseService.RemoveRangeAsync(request, token);
        return Ok(new { Message = SuccessMessages.ExerciseRemoved });
    }
}
