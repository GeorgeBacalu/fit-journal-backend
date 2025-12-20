using FitnessTracker.Core.Mappers;
using FitnessTracker.Infra.Config;
using FitnessTracker.Infra.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;

namespace FitnessTracker.Api;

public static class ServicesExtensions
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services) =>
        services.AddCors(options =>
            options.AddPolicy("AllowAll", policy =>
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader()));

    public static IServiceCollection AddDbContext(this IServiceCollection services) =>
        services.AddDbContext<FitnessTrackerContext>(options =>
            options.UseSqlServer(AppConfig.ConnectionStrings.FitnessTrackerDb));

    public static IServiceCollection AddAutoMapper(this IServiceCollection services) =>
        services.AddAutoMapper(_ => { }, typeof(UserMapper).Assembly);

    public static IServiceCollection AddSwagger(this IServiceCollection services) =>
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = "Fitness Tracker API",
                Description = "Health platform with real-time activity insights for accessible fitness progress",
                Version = "v1"
            });

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new()
            {
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header,
                Name = HeaderNames.Authorization,
                Scheme = JwtBearerDefaults.AuthenticationScheme.ToLower(),
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
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    []
                }
            });

            options.IncludeXmlComments($"{AppContext.BaseDirectory}{Assembly.GetExecutingAssembly().GetName().Name}.xml");
        });

    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = AppConfig.Auth.Issuer,
                ValidAudience = AppConfig.Auth.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfig.Auth.Secret))
            });

        return services.AddAuthorization();
    }

    public static void AddSerilog(this IHostBuilder host) =>
        host.UseSerilog((context, config) =>
            config.WriteTo.Console()
                  .ReadFrom.Configuration(context.Configuration));
}
