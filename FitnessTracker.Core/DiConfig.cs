using FitnessTracker.Core.Dtos.Requests.Auth;
using FitnessTracker.Core.Interfaces.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessTracker.Core;

public static class DiConfig
{
    public static IServiceCollection AddCore(this IServiceCollection services) =>
        services.Scan(scan =>
            scan.FromAssemblyOf<IBusinessService>()
                .AddClasses(filter => filter.AssignableTo<IBusinessService>())
                .AsImplementedInterfaces()
                .WithScopedLifetime());

    public static IServiceCollection AddValidators(this IServiceCollection services) =>
        services.AddValidatorsFromAssemblyContaining<RegisterValidator>()
                .AddFluentValidationAutoValidation();
}
