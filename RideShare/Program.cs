using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using RideShare.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
// Add services to the container.
builder.Services.AddControllers();

// Register the DbContext with SQL Server
builder.Services.AddDbContext<RideShareContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("myCon")));

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Register Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Bind dynamic port (for Railway deployment)
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000"; // Default to 5000 if no PORT variable exists
builder.WebHost.UseUrls($"http://*:{port}"); // Bind app to dynamic port


// Use CORS
app.UseCors("AllowAllOrigins");

if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI only in Development environment
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RideShare API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}
app.MapGet("/", () => Results.Ok("Welcome to RideShare API"));

app.MapGet("/health", () => Results.Ok("Healthy"));

app.UseHealthChecks("/health");

// Only enable HTTPS redirection in Production
//if (app.Environment.IsProduction())
//{
//    //app.UseHttpsRedirection();
//}

// Enable Authorization (if using Auth in your API)
app.UseAuthorization();

// Map API controllers
app.MapControllers();

// Start the app
app.Run();
