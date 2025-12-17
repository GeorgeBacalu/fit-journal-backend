using FitnessTracker.Core.Mappers;
using FitnessTracker.Core.Services;
using FitnessTracker.Infra.Config;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace FitnessTracker.Api;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        AppConfig.Init(configuration);

        ConfigureCors(services);
        ConfigureDbContext(services);
        ConfigureAutoMapper(services);
        ConfigureControllers(services);
        ConfigureSwagger(services);
        ConfigureAuth(services);

        services.AddInfra()
                .AddCore()
                .AddMiddlewares();
    }

    public void Configure(WebApplication app)
    {
        app.UseSwagger()
           .UseSwaggerUI()

           .UseCors("AllowAll")
           .UseHttpsRedirection()

           .UseMiddlewares()
           .UseAuthentication()
           .UseAuthorization();

        app.MapControllers();
    }

    private static void ConfigureCors(IServiceCollection services) =>
        services.AddCors(options =>
            options.AddPolicy("AllowAll", policy =>
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader()));

    private static void ConfigureDbContext(IServiceCollection services) =>
        services.AddDbContext<FitnessTrackerContext>(
            options => options.UseSqlServer(AppConfig.ConnectionStrings.FitnessTrackerDb));

    private static void ConfigureAutoMapper(IServiceCollection services) =>
        services.AddAutoMapper(_ => { }, typeof(UserMapper).Assembly);

    private static void ConfigureControllers(IServiceCollection services) =>
        services.AddControllers();

    private static void ConfigureSwagger(IServiceCollection services) =>
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = "Fitness Tracker API",
                Description = "Health platform with real-time activity insights for accessible fitness progress",
                Version = "v1"
            });

            options.AddSecurityDefinition("Bearer", new()
            {
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header,
                Name = "Authorization",
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(new()
            {
                {
                    new()
                    {
                        Reference = new()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });

            options.IncludeXmlComments($"{AppContext.BaseDirectory}/{Assembly.GetExecutingAssembly().GetName().Name}.xml");
        });

    private static void ConfigureAuth(IServiceCollection services)
    {
        services.AddAuthentication("Bearer").AddJwtBearer(
            options => options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = AppConfig.Auth.Issuer,
                ValidAudience = AppConfig.Auth.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfig.Auth.Secret))
            });

        services.AddAuthorization();
    }
}
