using FitnessTracker.App.Services;
using FitnessTracker.App.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessTracker.App.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
