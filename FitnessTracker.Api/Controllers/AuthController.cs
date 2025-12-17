using FitnessTracker.Core.Dtos.Requests.Auth;
using FitnessTracker.Core.Dtos.Responses.Auth;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Infra.Constants;
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
        return Created("", new { Message = SuccessMessages.UserRegistered });
    }

    /// <summary>Login existing user</summary>
    /// <param name="request">User login details</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Access and refresh tokens</returns>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken token = default) =>
        Ok(await authService.LoginAsync(request, token));
}
