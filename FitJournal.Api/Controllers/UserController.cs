using FitJournal.Core.Constants;
using FitJournal.Core.Dtos.Requests.Users;
using FitJournal.Core.Dtos.Responses.Users;
using FitJournal.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitJournal.Api.Controllers;

[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserController(IUserService userService) : BaseController
{
    private readonly IUserService _userService = userService;

    /// <summary>Get all users</summary>
    /// <param name="request">User pagination info</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>All users</returns>
    [HttpPost("all")]
    [ProducesResponseType(typeof(UsersResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UsersResponse>> GetAllAsync(UserPaginationRequest request, CancellationToken token = default) =>
        Ok(await _userService.GetAllAsync(request, token));

    /// <summary>Get user by ID</summary>
    /// <param name="id">Given user ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>User with given ID</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await _userService.GetByIdAsync(id, token));

    /// <summary>Edit current user</summary>
    /// <param name="request">Edited current user details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPut]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> EditAsync(EditUserRequest request, CancellationToken token = default)
    {
        await _userService.EditAsync(request, UserId, token);

        return Ok(new MessageResponse(SuccessMessages.Users.Edited));
    }
}
