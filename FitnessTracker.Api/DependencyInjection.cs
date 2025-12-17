using FitnessTracker.Infra.Middlewares;

namespace FitnessTracker.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddMiddlewares(this IServiceCollection services) =>
        services.AddTransient<ExceptionMiddleware>()
                .AddTransient<LoggingMiddleware>()
                .AddTransient<CachingMiddleware>();

    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app) =>
        app.UseMiddleware<ExceptionMiddleware>()
           .UseMiddleware<LoggingMiddleware>()
           .UseMiddleware<CachingMiddleware>();
}
