using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FitnessTracker.Infra.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services) =>
        services.Scan(scan =>
            scan.FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(classes => classes.InNamespaces(typeof(DependencyInjection).Namespace!))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
}
