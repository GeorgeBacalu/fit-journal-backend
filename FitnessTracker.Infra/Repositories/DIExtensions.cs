using FitnessTracker.Infra.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FitnessTracker.Infra.Repositories;

public static class DIExtensions
{
    public static IServiceCollection AddInfra(this IServiceCollection services) =>
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>))
            .Scan(scan =>
                scan.FromAssemblies(Assembly.GetExecutingAssembly())
                    .AddClasses(filter => filter.Where(type =>
                        type.Namespace!.StartsWith(typeof(DIExtensions).Namespace!)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());
}
