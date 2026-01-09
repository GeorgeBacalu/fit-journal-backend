using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.Users;
using FitnessTracker.Core.Interfaces.Services;
using FitnessTracker.Domain.Enums.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers.Admin;

[Authorize(Roles = nameof(Role.Admin))]
[Route("api/v{version:apiVersion}/[controller]")]
public class AdminUserController(IUserService userService) : BaseController
{
    private readonly IUserService _userService = userService;

    /// <summary>Edit user (admin)</summary>
    /// <param name="request">Edited user details</param>
    /// <param name="id">Edited user ID</param> 
    /// <param name="token">Cancellation token</param>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageResponse>> EditAsync(EditUserRequest request, Guid id, CancellationToken token = default)
    {
        await _userService.EditAsync(request, id, token);

        return Ok(new MessageResponse(SuccessMessages.Users.Edited));
    }
}
