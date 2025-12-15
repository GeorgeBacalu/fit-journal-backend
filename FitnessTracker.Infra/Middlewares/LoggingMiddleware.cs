using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FitnessTracker.Infra.Middlewares;

public class LoggingMiddleware(ILogger<LoggingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();
        logger.LogInformation($"\n[START] {context.Request.Method} {context.Request.Path}");
        await next(context);
        stopwatch.Stop();
        logger.LogInformation($"[FINISH] {context.Request.Method} {context.Request.Path} - {stopwatch.ElapsedMilliseconds} ms\n");
    }
}
