using FitnessTracker.Api;
using FitnessTracker.Core.Services;
using FitnessTracker.Infra.Config;
using FitnessTracker.Infra.Repositories;

var builder = WebApplication.CreateBuilder(args);

AppConfig.Init(builder.Configuration);

builder.Services
    .AddCorsPolicy()

    .AddDbContext()
    .AddAutoMapper()
    .AddInfra()

    .AddCore()
    .AddValidators()
    .AddMiddlewares()

    .AddSwagger()
    .AddAuth()

    .AddControllers();

builder.Host.AddSerilog();

var app = builder.Build();

app.UseSwagger()
   .UseSwaggerUI()

   .UseCors("AllowAll")
   .UseHttpsRedirection()

   .UseMiddlewares()
   .UseAuthentication()
   .UseAuthorization();

app.MapControllers();

await app.RunAsync();
