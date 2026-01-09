using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.WorkoutExercises;
using FitnessTracker.Core.Dtos.Responses.WorkoutExercises;
using FitnessTracker.Core.Interfaces.Services;
using FitnessTracker.Domain.Enums.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers.Admin;

[Authorize(Roles = nameof(Role.Admin))]
[Route("api/v{version:apiVersion}/[controller]")]
public class AdminWorkoutExerciseController(IWorkoutExerciseService workoutExerciseService) : BaseController
{
    private readonly IWorkoutExerciseService _workoutExerciseService = workoutExerciseService;

    /// <summary>Get all workout exercises</summary>
    /// <param name="request">Workout exercise pagination info</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>All workout exercises from workout</returns>
    [HttpPost("all")]
    [ProducesResponseType(typeof(UsersWorkoutExercisesResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UsersWorkoutExercisesResponse>> GetAllAsync(WorkoutExercisePaginationRequest request, CancellationToken token = default) =>
        Ok(await _workoutExerciseService.GetAllAsync(request, null, token));

    /// <summary>Get workout exercise by ID</summary>
    /// <param name="id">Given workout exercise ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Workout exercise with given ID</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(WorkoutExerciseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutExerciseResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await _workoutExerciseService.GetByIdAsync(id, null, token));

    /// <summary>Add new workout exercise</summary>
    /// <param name="request">Added workout exercise details</param>
    /// <param name="userId">Workout exercises's user ID</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost("{userId:guid}")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> AddAsync(AddWorkoutExerciseRequest request, Guid userId, CancellationToken token = default)
    {
        await _workoutExerciseService.AddAsync(request, userId, token);

        return Created(string.Empty, new MessageResponse(SuccessMessages.WorkoutExercises.Added));
    }

    /// <summary>Edit existing workout exercise</summary>
    /// <param name="request">Edited workout exercise details</param>
    /// <param name="userId">Workout exercises's user ID</param>
    /// <param name="token">Cancellation token</param>
    [HttpPut("{userId:guid}")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> EditAsync(EditWorkoutExerciseRequest request, Guid userId, CancellationToken token = default)
    {
        await _workoutExerciseService.EditAsync(request, userId, token);

        return Ok(new MessageResponse(SuccessMessages.WorkoutExercises.Edited));
    }

    /// <summary>Remove existing workout exercises</summary>
    /// <param name="request">Removed workout exercises IDs</param>
    /// <param name="userId">Workout exercises's user ID</param>
    /// <param name="token">Cancellation token</param>
    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> RemoveRangeAsync(RemoveWorkoutExercisesRequest request, Guid userId, CancellationToken token = default)
    {
        await _workoutExerciseService.RemoveRangeAsync(request, userId, token);

        return Ok(new MessageResponse(SuccessMessages.WorkoutExercises.RemovedRange));
    }
}
