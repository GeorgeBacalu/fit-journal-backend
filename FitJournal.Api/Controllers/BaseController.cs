using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Security.Claims;

namespace FitJournal.Api.Controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public abstract class BaseController : ControllerBase
{
    protected Guid UserId => Guid.TryParse(User.FindFirstValue("userId"), out var id) ? id : Guid.Empty;
}

public record MessageResponse(string Message);
public record ApiErrorResponse(string? Title, string? Detail);
