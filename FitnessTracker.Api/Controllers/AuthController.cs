using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.Auth;
using FitnessTracker.Core.Dtos.Responses.Auth;
using FitnessTracker.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController(IAuthService authService) : BaseController
{
    private readonly IAuthService _authService = authService;

    /// <summary>Register new user</summary>
    /// <param name="request">User registration details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost("register")]
    public async Task<ActionResult<object>> RegisterAsync(RegisterRequest request, CancellationToken token = default)
    {
        await _authService.RegisterAsync(request, token);

        return Created(string.Empty, new { Message = SuccessMessages.Users.Registered });
    }

    /// <summary>Login existing user</summary>
    /// <param name="request">User login details</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Access and refresh tokens</returns>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken token = default) =>
        Ok(await _authService.LoginAsync(request, token));

    /// <summary>Deactivate current user account</summary>
    /// <param name="token">Cancellation token</param>
    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<object>> DeleteAsync(CancellationToken token = default)
    {
        await _authService.DeleteAsync(UserId, token);

        return Ok(new { Message = SuccessMessages.Users.AccountDeactivated });
    }
}
