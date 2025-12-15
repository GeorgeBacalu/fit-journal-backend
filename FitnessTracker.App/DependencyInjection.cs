using FitnessTracker.App.Services;
using FitnessTracker.App.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessTracker.App;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
