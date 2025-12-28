using FitnessTracker.Core.Interfaces.Services.Admin;
using FitnessTracker.Domain.Enums;
using FitnessTracker.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers.Admin;

[Authorize(Roles = nameof(Role.Admin))]
[Route("api/[controller]")]
public class AdminAuthController(IAdminAuthService adminAuthService) : BaseController
{
    /// <summary>Deactivate user account (admin)</summary>
    /// <param name="id">Deactivated user ID</param> 
    /// <param name="token">Cancellation token</param>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<object>> DeleteAsync(Guid id, CancellationToken token = default)
    {
        await adminAuthService.DeleteAsync(id, token);

        return Ok(new { Message = SuccessMessages.Users.AccountDeactivated });
    }
}
