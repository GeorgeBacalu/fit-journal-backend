using FitnessTracker.Infra.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FitnessTracker.Infra.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var message = exception.InnerException?.Message ?? exception.Message;
            ProblemDetails problemDetails = exception switch
            {
                BadRequestException => new() { Title = "Bad request", Detail = message, Status = StatusCodes.Status400BadRequest },
                UnauthorizedException => new() { Title = "Unauthorized", Detail = message, Status = StatusCodes.Status401Unauthorized },
                ForbiddenException => new() { Title = "Forbidden", Detail = message, Status = StatusCodes.Status403Forbidden },
                NotFoundException => new() { Title = "Not found", Detail = message, Status = StatusCodes.Status404NotFound },
                _ => new() { Title = "Internal server error", Detail = message, Status = StatusCodes.Status500InternalServerError }
            };
            context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
    }
}
