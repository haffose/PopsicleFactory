using API.Repositories;
using API.Repositories.Interfaces;
using API.Services;
using API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register repositories (Singleton for in-memory storage)
builder.Services.AddSingleton<IPopsicleRepository, PopsicleRepository>();

// Register services
builder.Services.AddScoped<IPopsicleService, PopsicleService>();

// Add health checks
builder.Services.AddHealthChecks();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Add health check endpoint
app.MapHealthChecks("/health");

// Redirect root to Swagger for better routing experience
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();