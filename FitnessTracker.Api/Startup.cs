using FitnessTracker.App;
using FitnessTracker.App.Mappers;
using FitnessTracker.Infra;
using FitnessTracker.Infra.Config;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace FitnessTracker.Api;

public class Startup
{
    private const string CorsPolicyName = "AllowAll";

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
        ConfigureAuth(services);
        ConfigureCaching(services);

        services.AddInfrastructure()
                .AddCore()

                .AddTransient<LoggingMiddleware>()
                .AddTransient<ResponseCachingMiddleware>()
                .AddTransient<ExceptionHandlingMiddleware>();
    }

    public void Configure(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors(CorsPolicyName);
        app.UseHttpsRedirection();

        app.UseResponseCaching();
        app.UseMiddleware<LoggingMiddleware>();
        app.UseMiddleware<ResponseCachingMiddleware>();
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }

    private static void ConfigureCors(IServiceCollection services)
        => services.AddCors(options => options.AddPolicy(CorsPolicyName,
               policy => policy.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader()));

    private static void ConfigureDbContext(IServiceCollection services)
        => services.AddDbContext<FitnessTrackerContext>(
               options => options.UseSqlServer(AppConfig.ConnectionStrings.FitnessTrackerDb,
                   sqlOptions => sqlOptions.MigrationsAssembly("FitnessTracker.Infra")));

    private static void ConfigureAutoMapper(IServiceCollection services)
        => services.AddAutoMapper(_ => { },
            typeof(UserMapper),
            typeof(WorkoutMapper),
            typeof(ExerciseMapper));

    private static void ConfigureControllers(IServiceCollection services)
        => services.AddControllers();

    private static void ConfigureCaching(IServiceCollection services)
        => services.AddResponseCaching(options =>
        {
            options.MaximumBodySize = 1024;
            options.UseCaseSensitivePaths = true;
        });

    private static void ConfigureSwagger(IServiceCollection services)
        => services.AddSwaggerGen(options =>
           {
               options.SwaggerDoc("v1", new()
               {
                   Title = "Fitness Tracker API",
                   Description = "Health platform offering accessible fitness progress through real-time activity insights",
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
                       }, []
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
