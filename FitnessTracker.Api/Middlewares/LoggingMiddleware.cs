using System.Diagnostics;

namespace FitnessTracker.Api.Middlewares;

public class LoggingMiddleware(ILogger<LoggingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();
        logger.LogInformation("\n\n[START] {Method} {Path}", context.Request.Method, context.Request.Path);

        await next(context);

        stopwatch.Stop();
        logger.LogInformation("\n[END] {Method} {Path} - {ElapsedMilliseconds} ms\n", context.Request.Method, context.Request.Path, stopwatch.ElapsedMilliseconds);
    }
}
