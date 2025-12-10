using FitnessTracker.Infra.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                DbUpdateException when message.Contains("UNIQUE") => new() { Title = $"Duplicated {GetField(message)}", Detail = $"{Capitalize(GetField(message))} already taken", Status = StatusCodes.Status409Conflict },
                _ => new() { Title = "Internal server error", Detail = message, Status = StatusCodes.Status500InternalServerError }
            };
            context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
    }

    private static string GetField(string message) => message.Contains("Name") ? "name" : message.Contains("Email") ? "email" : "resource";

    private static string Capitalize(string message) => $"{message[0].ToString().ToUpperInvariant()}{message[1..]}";
}
