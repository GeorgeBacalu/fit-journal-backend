using Microsoft.Net.Http.Headers;

namespace FitnessTracker.Api.Middlewares;

public class CachingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Response.Headers[HeaderNames.Vary] = new[] { "Accept-Encoding" };
        context.Response.GetTypedHeaders().CacheControl = new() { MaxAge = TimeSpan.FromSeconds(30) };

        await next(context);
    }
}
