using FitnessTracker.Infra.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;

namespace FitnessTracker.Infra.Middlewares;

public class ExceptionMiddleware : IMiddleware
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
            var field = GetField(message);

            ProblemDetails problemDetails = exception switch
            {
                BadRequestException => new()
                {
                    Detail = message,
                    Status = StatusCodes.Status400BadRequest
                },
                UnauthorizedException => new()
                {
                    Detail = message,
                    Status = StatusCodes.Status401Unauthorized
                },
                ForbiddenException => new()
                {
                    Detail = message,
                    Status = StatusCodes.Status403Forbidden
                },
                NotFoundException => new()
                {
                    Detail = message,
                    Status = StatusCodes.Status404NotFound
                },
                DbUpdateException when message.Contains("UNIQUE") => new()
                {
                    Title = $"Duplicated {field}",
                    Detail = $"{Capitalize(field)} already taken",
                    Status = StatusCodes.Status409Conflict
                },
                DbUpdateException => new()
                {
                    Detail = message,
                    Status = StatusCodes.Status409Conflict
                },
                _ => new()
                {
                    Detail = message,
                    Status = StatusCodes.Status500InternalServerError
                }
            };

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }

    private static string GetField(string message)
        => UniqueConstraintMappings.FirstOrDefault(mapping => message.Contains(mapping.Key)).Value ?? "resource";

    private static string Capitalize(string message)
        => char.ToUpperInvariant(message[0]) + message[1..];

    private static readonly Dictionary<string, string> UniqueConstraintMappings = new()
    {
        ["Name"] = "name",
        ["Email"] = "email"
    };
}
