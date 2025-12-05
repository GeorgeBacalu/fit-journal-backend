using FitnessTracker.Infra.Config;

var builder = WebApplication.CreateBuilder(args);
AppConfig.Init(builder.Configuration);
builder.Services.AddCors(options => options.AddPolicy("Cors", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("X-Correlation-ID", "Content-Disposition")));
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
