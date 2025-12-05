using FitnessTracker.Infra.Config;
using FitnessTracker.Infra.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
AppConfig.Init(builder.Configuration);
builder.Services.AddCors(options => options.AddPolicy("Cors", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("X-Correlation-ID", "Content-Disposition")));
builder.Services.AddDbContext<FitnessTrackerContext>(options => options.UseSqlServer(AppConfig.ConnectionStrings.FitnessTrackerDb, sqlOptions => sqlOptions.MigrationsAssembly("FitnessTracker.Infra")));
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("Cors");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
