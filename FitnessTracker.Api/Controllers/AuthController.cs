using FitnessTracker.App.Services.Interfaces;
using FitnessTracker.App.Dtos.Requests.Auth;
using FitnessTracker.Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers;
[Route(ApiConstants.ApiAuth)]
public class AuthController(IAuthService authService) : BaseController
{
    /// <summary>Register new user</summary>
    /// <param name="request">User registration details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost("register")]
    public async Task<ActionResult<object>> RegisterAsync(RegisterRequest request, CancellationToken token = default)
    {
        await authService.RegisterAsync(request, token);
        return Created("", new { Message = SuccessMessageConstants.UserRegistered });
    }
}
