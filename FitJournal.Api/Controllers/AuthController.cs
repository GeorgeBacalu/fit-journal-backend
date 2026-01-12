using FitJournal.Core.Constants;
using FitJournal.Core.Dtos.Requests.Auth;
using FitJournal.Core.Dtos.Responses.Auth;
using FitJournal.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitJournal.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController(IAuthService authService) : BaseController
{
    private readonly IAuthService _authService = authService;

    /// <summary>Register new user</summary>
    /// <param name="request">User registration details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost("register")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MessageResponse>> RegisterAsync(RegisterRequest request, CancellationToken token = default)
    {
        await _authService.RegisterAsync(request, token);

        return Created(string.Empty, new MessageResponse(SuccessMessages.Users.Registered));
    }

    /// <summary>Login existing user</summary>
    /// <param name="request">User login details</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Access and refresh tokens</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken token = default) =>
        Ok(await _authService.LoginAsync(request, token));

    /// <summary>Refresh access token using refresh token</summary>
    /// <param name="request">Refresh token details</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Access and refresh tokens</returns>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LoginResponse>> RefreshAsync(RefreshRequest request, CancellationToken token = default) =>
        Ok(await _authService.RefreshAsync(request, token));

    /// <summary>Deactivate current user account</summary>
    /// <param name="token">Cancellation token</param>
    [Authorize]
    [HttpDelete]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> DeleteAsync(CancellationToken token = default)
    {
        await _authService.DeleteAsync(UserId, token);

        return Ok(new MessageResponse(SuccessMessages.Users.AccountDeactivated));
    }
}
