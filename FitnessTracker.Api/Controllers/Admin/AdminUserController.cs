using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.Users;
using FitnessTracker.Core.Interfaces.Services.Admin;
using FitnessTracker.Domain.Enums.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers.Admin;

[Authorize(Roles = nameof(Role.Admin))]
[Route("api/v{version:apiVersion}/[controller]")]
public class AdminUserController(IAdminUserService adminUserService) : BaseController
{
    private readonly IAdminUserService _adminUserService = adminUserService;

    /// <summary>Edit user (admin)</summary>
    /// <param name="request">Edited user details</param>
    /// <param name="id">Edited user ID</param> 
    /// <param name="token">Cancellation token</param>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<object>> EditAsync(EditUserRequest request, Guid id, CancellationToken token = default)
    {
        await _adminUserService.EditAsync(request, id, token);

        return Ok(new { Message = SuccessMessages.Users.Edited });
    }
}
