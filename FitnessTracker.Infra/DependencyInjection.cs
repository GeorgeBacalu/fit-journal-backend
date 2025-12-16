using FitnessTracker.Infra.Repositories;
using FitnessTracker.Infra.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessTracker.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWorkoutRepository, WorkoutRepository>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();

        return services;
    }
}
