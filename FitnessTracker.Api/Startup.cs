using FitnessTracker.App.DI;
using FitnessTracker.App.Mappers;
using FitnessTracker.Infra.Config;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.DI;
using FitnessTracker.Infra.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace FitnessTracker.Api;

public class Startup
{
    private const string CorsPolicyName = "Cors";

    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        AppConfig.Init(configuration);
    }

    public void ConfigureServices(IServiceCollection services)
    {
        ConfigureCors(services);
        ConfigureDbContext(services);
        ConfigureAutoMapper(services);
        ConfigureControllers(services);
        ConfigureSwagger(services);
        ConfigureAuthentication(services);

        services.AddInfraRepositories();
        services.AddAppServices();

        services.AddTransient<LoggingMiddleware>();
        services.AddTransient<ExceptionHandlingMiddleware>();
    }

    public void Configure(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors(CorsPolicyName);
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<LoggingMiddleware>();
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.MapControllers();
    }

    private static void ConfigureCors(IServiceCollection services)
        => services.AddCors(options => options.AddPolicy(CorsPolicyName,
               builder => builder.AllowAnyOrigin()
                                 .AllowAnyMethod()
                                 .AllowAnyHeader()
                                 .WithExposedHeaders("X-Correlation-ID", "Content-Disposition")));

    private static void ConfigureDbContext(IServiceCollection services)
        => services.AddDbContext<FitnessTrackerContext>(
               options => options.UseSqlServer(AppConfig.ConnectionStrings.FitnessTrackerDb,
                   sqlOptions => sqlOptions.MigrationsAssembly("FitnessTracker.Infra")));

    private static void ConfigureAutoMapper(IServiceCollection services)
        => services.AddAutoMapper(_ => { }, typeof(UserMapper));

    private static void ConfigureControllers(IServiceCollection services)
        => services.AddControllers();

    private static void ConfigureSwagger(IServiceCollection services)
        => services.AddSwaggerGen(options =>
           {
               options.SwaggerDoc("v1", new()
               {
                   Title = "Fitness Tracker API",
                   Description = "Health platform offering accessible fitness progress through real-time activity insights",
                   Version = "1.0"
               });

               options.AddSecurityDefinition("Bearer", new()
               {
                   Name = "Authorization",
                   In = ParameterLocation.Header,
                   Type = SecuritySchemeType.Http,
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
                       }, []
                   }
               });

               options.IncludeXmlComments($"{AppContext.BaseDirectory}/{Assembly.GetExecutingAssembly().GetName().Name}.xml");
           });

    private static void ConfigureAuthentication(IServiceCollection services)
        => services.AddAuthentication("Bearer").AddJwtBearer(
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
}
