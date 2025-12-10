using FitnessTracker.Infra.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessTracker.Test.Integration;

public class WebAppFactory(DbFixture fixture) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder) => builder.ConfigureServices(services => services.AddScoped(_ => new FitnessTrackerContext(fixture.DbOptions)));
}
