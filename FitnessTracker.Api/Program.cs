using FitnessTracker.App.Mappers;
using FitnessTracker.App.Services;
using FitnessTracker.App.Services.Interfaces;
using FitnessTracker.Infra.Config;
using FitnessTracker.Infra.Context;
using FitnessTracker.Infra.Middlewares;
using FitnessTracker.Infra.Repositories;
using FitnessTracker.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
AppConfig.Init(builder.Configuration);

builder.Services.AddCors(options => options.AddPolicy("Cors",
    builder => builder.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .WithExposedHeaders("X-Correlation-ID", "Content-Disposition")));

builder.Services.AddDbContext<FitnessTrackerContext>(
    options => options.UseSqlServer(AppConfig.ConnectionStrings.FitnessTrackerDb,
        sqlOptions => sqlOptions.MigrationsAssembly("FitnessTracker.Infra")));

builder.Services.AddAutoMapper(_ => { }, typeof(UserMapper));
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Fitness Tracker API",
        Description = "Fitness Tracker",
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
                Reference = new() { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            }, []
        }
    });
    options.IncludeXmlComments($"{AppContext.BaseDirectory}/{Assembly.GetExecutingAssembly().GetName().Name}.xml");
}).AddAuthentication("Bearer")
  .AddJwtBearer(options => options.TokenValidationParameters = new()
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

builder.Services.AddTransient<LoggingMiddleware>()
                .AddTransient<ExceptionHandlingMiddleware>()

                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IUserRepository, UserRepository>()

                .AddScoped<IAuthService, AuthService>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("Cors");
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
app.Run();
