using FitJournal.Core.Dtos.Requests.Auth;
using FitJournal.Core.Interfaces.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace FitJournal.Core;

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
