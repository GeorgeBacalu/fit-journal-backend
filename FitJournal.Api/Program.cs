using Asp.Versioning.ApiExplorer;
using FitJournal.Api;
using FitJournal.Api.Extensions;
using FitJournal.Core;
using FitJournal.Core.Config;
using FitJournal.Infra;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

AppConfig.Init(builder.Configuration);

builder.Services
    .AddCorsPolicy()
    .AddAutoMapper()
    .AddSwagger()
    .AddAuth()
    .AddApiVersions()
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
       foreach (var groupName in descriptions.Select(d => d.GroupName))
           options.SwaggerEndpoint($"/swagger/{groupName}/swagger.json", $"FitJournal API {groupName.ToUpperInvariant()}");
   })
   .UseCors("AllowAll")
   .UseSerilogRequestLogging(options => options.MessageTemplate = "{RequestMethod} {RequestPath} {StatusCode} ({Elapsed:0.00} ms)")
   .UseHttpsRedirection()
   .UseMiddlewares()
   .UseAuthentication()
   .UseAuthorization();

app.MapControllers();

await app.RunAsync();
