using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace FitnessTracker.Infra.Middlewares;

public class ResponseCachingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue
        {
            Public = true,
            MaxAge = TimeSpan.FromMinutes(10)
        };
        context.Response.Headers[HeaderNames.Vary] = new[] { "Accept-Encoding" };

        await next(context);
    }
}
