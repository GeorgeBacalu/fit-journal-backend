using FitJournal.Core.Constants;
using FitJournal.Core.Interfaces.Services;
using FitJournal.Domain.Enums.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitJournal.Api.Controllers.Admin;

[Authorize(Roles = nameof(Role.Admin))]
[Route("api/v{version:apiVersion}/[controller]")]
public class AdminAuthController(IAuthService authService) : BaseController
{
    private readonly IAuthService _authService = authService;

    /// <summary>Deactivate user account</summary>
    /// <param name="id">Deactivated user ID</param> 
    /// <param name="token">Cancellation token</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> DeleteAsync(Guid id, CancellationToken token = default)
    {
        await _authService.DeleteAsync(id, token);

        return Ok(new MessageResponse(SuccessMessages.Users.AccountDeactivated));
    }
}
