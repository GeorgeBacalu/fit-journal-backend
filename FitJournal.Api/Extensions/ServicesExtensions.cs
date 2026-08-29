using FitJournal.Api.Extensions;
using FitJournal.Api.OpenApi;
using FitJournal.Core.Config;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Core.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace FitJournal.Api.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services) =>
        services.AddCors(options =>
            options.AddPolicy("AllowAll", policy =>
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader()));

    public static IServiceCollection AddAutoMapper(this IServiceCollection services) =>
        services.AddAutoMapper(_ => { }, typeof(UserMapper).Assembly);

    public static IServiceCollection AddSwagger(this IServiceCollection services) =>
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new()
            {
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme.ToLower(),
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(new()
            {
                [new()
                {
                    Reference = new()
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    }
                }] = []
            });

            options.IncludeXmlComments($"{AppContext.BaseDirectory}{Assembly.GetExecutingAssembly().GetName().Name}.xml");
        });

    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = AppConfig.Auth.Issuer,
                    ValidAudience = AppConfig.Auth.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfig.Auth.Secret))
                };

                options.Events = new()
                {
                    OnTokenValidated = async context =>
                    {
                        if (!Guid.TryParse(context.Principal?.FindFirstValue("userId"), out var id))
                        {
                            context.Fail("Invalid token");
                            return;
                        }

                        var unitOfWork = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
                        if (await unitOfWork.Users.GetByIdAsync(id, context.HttpContext.RequestAborted) == null)
                            context.Fail("User is inactive");
                    }
                };
            });

        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        var google = configuration.GetSection("ExternalAuth:Google");
        var microsoft = configuration.GetSection("ExternalAuth:Microsoft");
        services.AddAuthentication()
            .AddCookie("External")
            .AddGoogle(GoogleDefaults.AuthenticationScheme, options => { options.SignInScheme = "External"; options.ClientId = google["ClientId"] ?? string.Empty; options.ClientSecret = google["ClientSecret"] ?? string.Empty; })
            .AddMicrosoftAccount(MicrosoftAccountDefaults.AuthenticationScheme, options => { options.SignInScheme = "External"; options.ClientId = microsoft["ClientId"] ?? string.Empty; options.ClientSecret = microsoft["ClientSecret"] ?? string.Empty; });

        return services.AddAuthorization();
    }

    public static IServiceCollection AddApiVersions(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new(1, 0);
            options.ReportApiVersions = true;
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services.ConfigureOptions<SwaggerConfigOptions>();
    }

    public static void AddSerilog(this IHostBuilder host) =>
        host.UseSerilog((context, config) =>
            config.ReadFrom.Configuration(context.Configuration));
}
