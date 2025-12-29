using Asp.Versioning;
using FitnessTracker.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Api.Controllers.Admin;

[Authorize(Roles = nameof(Role.Admin))]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AdminProgressLogController : BaseController
{
}
