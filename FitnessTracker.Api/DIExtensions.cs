using FitnessTracker.Infra.Middlewares;

namespace FitnessTracker.Api;

public static class DiExtensions
{
    public static IServiceCollection AddMiddlewares(this IServiceCollection services) =>
        services.AddTransient<ExceptionMiddleware>()
                .AddTransient<CachingMiddleware>()
                .AddTransient<LoggingMiddleware>();

    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app) =>
        app.UseMiddleware<ExceptionMiddleware>()
           .UseMiddleware<CachingMiddleware>()
           .UseMiddleware<LoggingMiddleware>();
}
