using FitnessTracker.Api;
using FitnessTracker.Api.Extensions;
using FitnessTracker.Core;
using FitnessTracker.Core.Config;
using FitnessTracker.Infra;

var builder = WebApplication.CreateBuilder(args);

AppConfig.Init(builder.Configuration);

builder.Services
    .AddCorsPolicy()
    .AddAutoMapper()
    .AddSwagger()
    .AddAuth()

    .AddInfra()
    .AddCore()
    .AddValidators()
    .AddMiddlewares()
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
