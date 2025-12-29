using Asp.Versioning;
using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.Users;
using FitnessTracker.Core.Interfaces.Services;
using FitnessTracker.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers.Admin;

[Authorize(Roles = nameof(Role.Admin))]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AdminUserController(IUserService userService) : BaseController
{
    private readonly IUserService _userService = userService;

    /// <summary>Edit user (admin)</summary>
    /// <param name="request">Edited user details</param>
    /// <param name="id">Edited user ID</param> 
    /// <param name="token">Cancellation token</param>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<object>> EditAsync(EditUserRequest request, Guid id, CancellationToken token = default)
    {
        await _userService.EditAsync(request, id, token);

        return Ok(new { Message = SuccessMessages.Users.Edited });
    }
}
