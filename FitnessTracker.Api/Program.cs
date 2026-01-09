using Asp.Versioning.ApiExplorer;
using FitnessTracker.Api;
using FitnessTracker.Api.Extensions;
using FitnessTracker.Core;
using FitnessTracker.Core.Config;
using FitnessTracker.Infra;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

AppConfig.Init(builder.Configuration);

builder.Services
    .AddCorsPolicy()
    .AddAutoMapper()
    .AddSwagger()
    .AddApiVersions()
    .AddAuth()
    .AddInfra()
    .AddCore()
    .AddValidators()
    .AddMiddlewares()
    .AddControllers();

builder.Host.AddSerilog();

var app = builder.Build();

app.UseSwagger()
   .UseSwaggerUI(options =>
   {
       var descriptions = app.Services.GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions;
       foreach (var groupName in descriptions.Select(description => description.GroupName))
           options.SwaggerEndpoint($"/swagger/{groupName}/swagger.json", $"Fitness Tracker API {groupName.ToUpperInvariant()}");
   })
   .UseCors("AllowAll")
   .UseSerilogRequestLogging(options => options.MessageTemplate = "{RequestMethod} {RequestPath} {StatusCode} ({Elapsed:0.00} ms)")
   .UseHttpsRedirection()
   .UseMiddlewares()
   .UseAuthentication()
   .UseAuthorization();

app.MapControllers();

await app.RunAsync();
