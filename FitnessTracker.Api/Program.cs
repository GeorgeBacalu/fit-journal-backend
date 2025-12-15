using FitnessTracker.Api;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config)
    => config.WriteTo.Console()
             .ReadFrom.Configuration(context.Configuration));

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app);
app.Run();
