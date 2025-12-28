using FitnessTracker.Core.Config;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessTracker.Infra;

public static class DiConfig
{
    public static IServiceCollection AddInfra(this IServiceCollection services) =>
        services.AddInfra(options => options.UseSqlServer(AppConfig.ConnectionStrings.FitnessTrackerDb));

    public static IServiceCollection AddInfra(this IServiceCollection services, Action<DbContextOptionsBuilder> dbOptions) =>
        services.AddDbContext<AppDbContext>(dbOptions)
                .AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>))
                .AddScoped<IUnitOfWork, UnitOfWork>();
}
