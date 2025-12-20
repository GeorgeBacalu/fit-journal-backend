using FitnessTracker.Core.Dtos.Requests.Users;
using FitnessTracker.Core.Dtos.Responses.Users;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Domain.Enums;
using FitnessTracker.Infra.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Authorize]
[Route("/api/[controller]")]
public class UserController(IUserService userService) : BaseController
{
    /// <summary>Get all users</summary>
    /// <param name="token">Cancellation token</param>
    /// <returns>All users</returns>
    [HttpGet]
    public async Task<ActionResult<UsersResponse>> GetAllAsync(CancellationToken token = default) =>
        Ok(await userService.GetAllAsync(token));

    /// <summary>Get user by ID</summary>
    /// <param name="id">Given user ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>User with given ID</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Ok(await userService.GetByIdAsync(id, token));

    /// <summary>Edit current user</summary>
    /// <param name="request">Edited user details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPut]
    public async Task<ActionResult<object>> EditAsync(EditUserRequest request, CancellationToken token = default)
    {
        await userService.EditAsync(request, UserId, token);
        
        return Ok(new { Message = ResponseMessages.Users.Edited });
    }

    /// <summary>Edit user (admin)</summary>
    /// <param name="request">Edited user details</param>
    /// <param name="id">Edited user ID</param> 
    /// <param name="token">Cancellation token</param>
    [Authorize(Roles = nameof(Role.Admin))]
    [HttpPut("admin/{id:guid}")]
    public async Task<ActionResult<object>> EditAsync(EditUserRequest request, Guid id, CancellationToken token = default)
    {
        await userService.EditAsync(request, id, token);

        return Ok(new { Message = ResponseMessages.Users.Edited });
    }
}
