using FitnessTracker.Core.Dtos.Requests.Auth;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FitnessTracker.Core.Services;

public static class DIExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services) =>
        services.Scan(scan =>
            scan.FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(filter => filter.Where(type =>
                    type.Namespace!.StartsWith(typeof(DIExtensions).Namespace!)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

    public static IServiceCollection AddValidators(this IServiceCollection services) =>
        services.AddValidatorsFromAssemblyContaining<RegisterValidator>()
                .AddFluentValidationAutoValidation();
}
