using FitnessTracker.Core.Dtos.Requests.Users;
using FitnessTracker.Core.Dtos.Responses.Users;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Infra.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Route("/api/[controller]")]
public class UserController(IUserService userService) : BaseController
{
    /// <summary>Get all users</summary>
    /// <param name="token">Cancellation token</param>
    /// <returns>List of all users</returns>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UsersResponse>> GetAllAsync(CancellationToken token = default) =>
        Ok(await userService.GetAllAsync(token));

    /// <summary>View selected user's personal info</summary>
    /// <param name="id">Selected user's ID</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Selected user's personal info</returns>
    [Authorize]
    [HttpGet("profile/{id}")]
    public async Task<ActionResult<ProfileResponse>> GetProfileAsync(Guid id, CancellationToken token = default) =>
        Ok(await userService.GetProfileAsync(id, token));

    /// <summary>Update user personal info</summary>
    /// <param name="request">Updated user personal info</param>
    /// <param name="token">Cancellation token</param>
    [Authorize]
    [HttpPatch("profile")]
    public async Task<ActionResult<object>> UpdateProfileAsync(UpdateProfileRequest request, CancellationToken token = default)
    {
        await userService.UpdateProfileAsync(request, UserId, token);
        return Ok(new { Message = SuccessMessages.ProfileUpdated });
    }
}
