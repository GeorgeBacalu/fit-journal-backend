using Microsoft.Net.Http.Headers;

namespace FitJournal.Api.Middlewares;

public class CachingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Response.Headers[HeaderNames.Vary] = new[] { "Accept-Encoding", "Authorization" };
        context.Response.Headers[HeaderNames.Pragma] = "no-cache";
        context.Response.Headers[HeaderNames.Expires] = "0";
        context.Response.GetTypedHeaders().CacheControl = new()
        {
            NoCache = true,
            NoStore = true,
            Private = true,
            MustRevalidate = true
        };

        await next(context);
    }
}
