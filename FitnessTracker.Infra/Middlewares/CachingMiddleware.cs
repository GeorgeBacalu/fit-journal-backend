using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace FitnessTracker.Infra.Middlewares;

public class CachingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Response.GetTypedHeaders().CacheControl = new() { MaxAge = TimeSpan.FromSeconds(30) };
        context.Response.Headers[HeaderNames.Vary] = new[] { "Accept-Encoding" };

        await next(context);
    }
}
