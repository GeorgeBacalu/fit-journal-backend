using FitnessTracker.Infra.Repositories;
using FitnessTracker.Infra.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessTracker.Infra.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfraRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
