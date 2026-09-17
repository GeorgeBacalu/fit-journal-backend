using Asp.Versioning.ApiExplorer;
using FitJournal.Api;
using FitJournal.Api.Extensions;
using FitJournal.Core;
using FitJournal.Core.Config;
using FitJournal.Infra;
using Serilog;
using FitJournal.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

AppConfig.Init(builder.Configuration);

builder.Services
    .AddCorsPolicy(builder.Configuration)
    .AddAutoMapper()
    .AddSwagger()
    .AddAuth(builder.Configuration)
    .AddApiVersions()
    .AddInfra()
    .AddCore()
    .AddValidators()
    .AddMiddlewares()
    .AddAzureRuntime(builder.Configuration)
    .AddControllers();

builder.Host.AddSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseSwagger()
       .UseSwaggerUI(options =>
       {
           var descriptions = app.Services.GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions;
           foreach (var groupName in descriptions.Select(d => d.GroupName))
               options.SwaggerEndpoint($"/swagger/{groupName}/swagger.json", $"FitJournal API {groupName.ToUpperInvariant()}");
       });

if (builder.Configuration.GetValue<bool>("Database:ApplyMigrations"))
{
    await using var scope = app.Services.CreateAsyncScope();
    await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();
}

app.UseCors("Frontend")
   .UseSerilogRequestLogging(options => options.MessageTemplate = "{RequestMethod} {RequestPath} {StatusCode} ({Elapsed:0.00} ms)")
   .UseHttpsRedirection()
   .UseMiddlewares()
   .UseAuthentication()
   .UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = check => check.Tags.Contains("ready") });

await app.RunAsync();
