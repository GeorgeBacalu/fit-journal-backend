using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.Exercises;
using FitnessTracker.Core.Dtos.Responses.Exercises;
using FitnessTracker.Core.Interfaces.Services;
using FitnessTracker.Domain.Enums.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public class ExerciseController(IExerciseService exerciseService) : BaseController
{
    private readonly IExerciseService _exerciseService = exerciseService;

    /// <summary>Get all exercises</summary>
    /// <param name="request">Exercise pagination info</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>All exercises</returns>
    [HttpPost("all")]
    [ProducesResponseType(typeof(ExercisesResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ExercisesResponse>> GetAllAsync(ExercisePaginationRequest request, CancellationToken token = default) =>
        Ok(await _exerciseService.GetAllAsync(request, token));

    /// <summary>Get exercise by ID</summary>
    /// <param name="id">Given exercise ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Exercise with given ID</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ExerciseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExerciseResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await _exerciseService.GetByIdAsync(id, token));

    /// <summary>Add new exercise</summary>
    /// <param name="request">Added exercise details</param>
    /// <param name="token">Cancellation token</param>
    [Authorize(Roles = nameof(Role.Admin))]
    [HttpPost]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MessageResponse>> AddAsync(AddExerciseRequest request, CancellationToken token = default)
    {
        await _exerciseService.AddAsync(request, token);

        return Created(string.Empty, new MessageResponse(SuccessMessages.Exercises.Added));
    }

    /// <summary>Edit existing exercise</summary>
    /// <param name="request">Edited exercise details</param>
    /// <param name="token">Cancellation token</param>
    [Authorize(Roles = nameof(Role.Admin))]
    [HttpPut]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MessageResponse>> EditAsync(EditExerciseRequest request, CancellationToken token = default)
    {
        await _exerciseService.EditAsync(request, token);

        return Ok(new MessageResponse(SuccessMessages.Exercises.Edited));
    }

    /// <summary>Delete existing exercises</summary>
    /// <param name="request">Deleted exercises IDs</param>
    /// <param name="token">Cancellation token</param>
    [Authorize(Roles = nameof(Role.Admin))]
    [HttpDelete]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> RemoveRangeAsync(RemoveExercisesRequest request, CancellationToken token = default)
    {
        await _exerciseService.RemoveRangeAsync(request, token);

        return Ok(new MessageResponse(SuccessMessages.Exercises.RemovedRange));
    }
}
