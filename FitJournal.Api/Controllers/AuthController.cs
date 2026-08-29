using FitJournal.Core.Constants;
using FitJournal.Core.Dtos.Requests.Auth;
using FitJournal.Core.Dtos.Responses.Auth;
using FitJournal.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace FitJournal.Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController(IAuthService authService, IConfiguration configuration, IDistributedCache cache) : BaseController
{
    private readonly IAuthService _authService = authService;
    private readonly IConfiguration _configuration = configuration;
    private readonly IDistributedCache _cache = cache;

    [HttpGet("external/{provider}")]
    public IActionResult ExternalLogin(string provider) => Challenge(new AuthenticationProperties { RedirectUri = Url.Action(nameof(ExternalCallback)) }, provider.ToLowerInvariant() switch
    {
        "google" => GoogleDefaults.AuthenticationScheme,
        "microsoft" => MicrosoftAccountDefaults.AuthenticationScheme,
        _ => throw new BadHttpRequestException("Supported providers are Google and Microsoft.")
    });

    [HttpGet("external/callback")]
    public async Task<IActionResult> ExternalCallback(CancellationToken token)
    {
        var result = await HttpContext.AuthenticateAsync("External");
        var email = result.Principal?.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(email)) return BadRequest("The provider did not return an email address.");
        var tokens = await _authService.ExternalLoginAsync(email, result.Principal?.FindFirstValue(ClaimTypes.Name) ?? string.Empty, token);
        await HttpContext.SignOutAsync("External");
        var frontendUrl = _configuration["ExternalAuth:FrontendUrl"] ?? "https://localhost:4200";
        var code = Convert.ToHexString(System.Security.Cryptography.RandomNumberGenerator.GetBytes(32));
        await _cache.SetStringAsync($"oauth:{code}", JsonSerializer.Serialize(tokens), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) }, token);
        return Redirect($"{frontendUrl.TrimEnd('/')}/oauth-callback?code={Uri.EscapeDataString(code)}");
    }

    [HttpPost("external/exchange")]
    public async Task<ActionResult<LoginResponse>> ExchangeExternalCodeAsync([FromBody] ExternalCodeRequest request, CancellationToken token)
    {
        var key = $"oauth:{request.Code}";
        var value = await _cache.GetStringAsync(key, token);
        if (value == null) return Unauthorized();
        await _cache.RemoveAsync(key, token);
        return Ok(JsonSerializer.Deserialize<LoginResponse>(value));
    }

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

    /// <summary>Change current user password</summary>
    /// <param name="request">Password change details</param>
    /// <param name="token">Cancellation token</param>
    [Authorize]
    [HttpPost("change-password")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken token = default)
    {
        await _authService.ChangePasswordAsync(request, UserId, token);

        return Ok(new MessageResponse(SuccessMessages.Users.PasswordChanged));
    }

    /// <summary>Send password reset email</summary>
    /// <param name="request">Forgot password details</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost("forgot-password")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken token = default)
    {
        await _authService.ForgotPasswordAsync(request, token);

        return Ok(new MessageResponse(SuccessMessages.Users.PasswordResetEmailSent));
    }

    /// <summary>Reset password</summary>
    /// <param name="request">Reset password token and new password</param>
    /// <param name="token">Cancellation token</param>
    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken token = default)
    {
        await _authService.ResetPasswordAsync(request, token);

        return Ok(new MessageResponse(SuccessMessages.Users.PasswordReset));
    }

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

public record ExternalCodeRequest(string Code);
