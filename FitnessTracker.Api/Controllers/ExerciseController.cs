using Asp.Versioning;
using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.Exercises;
using FitnessTracker.Core.Dtos.Responses.Exercises;
using FitnessTracker.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Authorize]
[ApiVersion("1.0")]
[Route("/api/v{version:apiVersion}/[controller]")]
public class ExerciseController(IExerciseService exerciseService) : BaseController
{
    private readonly IExerciseService _exerciseService = exerciseService;

    /// <summary>Get all exercises</summary>
    /// <param name="token">Cancellation token</param>
    /// <returns>All exercises</returns>
    [HttpGet]
    public async Task<ActionResult<ExercisesResponse>> GetAllAsync(CancellationToken token = default) =>
        Ok(await _exerciseService.GetAllAsync(token));

    /// <summary>Get exercise by ID</summary>
    /// <param name="id">Given exercise ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Exercise with given ID</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ExerciseResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await _exerciseService.GetByIdAsync(id, token));

    /// <summary>Add new exercise</summary>
    /// <param name="request">Added exercise details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost]
    public async Task<ActionResult<object>> AddAsync(AddExerciseRequest request, CancellationToken token = default)
    {
        await _exerciseService.AddAsync(request, token);

        return Created(string.Empty, new { Message = SuccessMessages.Exercises.Added });
    }

    /// <summary>Edit existing exercise</summary>
    /// <param name="request">Edited exercise details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPut]
    public async Task<ActionResult<object>> EditAsync(EditExerciseRequest request, CancellationToken token = default)
    {
        await _exerciseService.EditAsync(request, token);

        return Ok(new { Message = SuccessMessages.Exercises.Edited });
    }

    /// <summary>Delete existing exercises</summary>
    /// <param name="request">Deleted exercises IDs</param>
    /// <param name="token">Cancellation token</param>
    [HttpDelete]
    public async Task<ActionResult<object>> RemoveRangeAsync(RemoveExercisesRequest request, CancellationToken token = default)
    {
        await _exerciseService.RemoveRangeAsync(request, token);

        return Ok(new { Message = SuccessMessages.Exercises.RemovedRange });
    }
}
