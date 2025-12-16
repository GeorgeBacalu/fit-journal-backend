using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FitnessTracker.Core.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services) =>
        services.Scan(scan =>
            scan.FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(classes => classes.InNamespaces(typeof(DependencyInjection).Namespace!))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
}
