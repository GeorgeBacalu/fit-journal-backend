using FitnessTracker.Core.Dtos.Requests.Auth;
using FitnessTracker.Core.Dtos.Responses.Auth;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Domain.Enums;
using FitnessTracker.Infra.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Route("/api/[controller]")]
public class AuthController(IAuthService authService) : BaseController
{
    /// <summary>Register new user</summary>
    /// <param name="request">User registration details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost("register")]
    public async Task<ActionResult<object>> RegisterAsync(RegisterRequest request, CancellationToken token = default)
    {
        await authService.RegisterAsync(request, token);

        return Created(string.Empty, new { Message = ResponseMessages.Users.Registered });
    }

    /// <summary>Login existing user</summary>
    /// <param name="request">User login details</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Access and refresh tokens</returns>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken token = default) =>
        Ok(await authService.LoginAsync(request, token));

    /// <summary>Deactivate current user account</summary>
    /// <param name="token">Cancellation token</param>
    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<object>> DeleteAsync(CancellationToken token = default)
    {
        await authService.DeleteAsync(UserId, token);

        return Ok(new { Message = ResponseMessages.Users.AccountDeactivated });
    }

    /// <summary>Deactivate user account (admin)</summary>
    /// <param name="id">Deactivated user ID</param> 
    /// <param name="token">Cancellation token</param>
    [Authorize(Roles = nameof(Role.Admin))]
    [HttpDelete("admin/{id:guid}")]
    public async Task<ActionResult<object>> DeleteAsync(Guid id, CancellationToken token = default)
    {
        await authService.DeleteAsync(id, token);

        return Ok(new { Message = ResponseMessages.Users.AccountDeactivated });
    }
}
