using FitJournal.Core.Config;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Infra.Context;
using FitJournal.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FitJournal.Infra;

public static class DiConfig
{
    public static IServiceCollection AddInfra(this IServiceCollection services) =>
        services.AddInfra(options => options.UseSqlServer(AppConfig.ConnectionStrings.FitJournalDb));

    public static IServiceCollection AddInfra(this IServiceCollection services, Action<DbContextOptionsBuilder> dbOptions) =>
        services.AddDbContext<AppDbContext>(dbOptions)
                .AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>))
                .AddScoped<IUnitOfWork, UnitOfWork>();
}
