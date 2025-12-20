using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Api.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    protected Guid UserId => Guid.TryParse(User.FindFirstValue("userId"), out var id) ? id : default;
}
