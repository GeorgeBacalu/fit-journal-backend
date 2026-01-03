using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.Users;
using FitnessTracker.Core.Dtos.Responses.Users;
using FitnessTracker.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

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
    public async Task<ActionResult<UsersResponse>> GetAllAsync(UserPaginationRequest request, CancellationToken token = default) =>
        Ok(await _userService.GetAllAsync(request, token));

    /// <summary>Get user by ID</summary>
    /// <param name="id">Given user ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>User with given ID</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await _userService.GetByIdAsync(id, token));

    /// <summary>Edit current user</summary>
    /// <param name="request">Edited current user details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPut]
    public async Task<ActionResult<object>> EditAsync(EditUserRequest request, CancellationToken token = default)
    {
        await _userService.EditAsync(request, UserId, token);

        return Ok(new { Message = SuccessMessages.Users.Edited });
    }
}
