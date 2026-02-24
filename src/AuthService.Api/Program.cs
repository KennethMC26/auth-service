using AuthService.Persistence.Data;
using AuthService.Api.Extensions;
using Microsoft.EntityFrameworkCore;

// Configuración para manejar fechas en PostgreSQL
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios básicos
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORRECCIÓN: Usamos el nombre exacto de tu método en la extensión
builder.Services.AddPersistenceServices(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

// Configuración del pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Nota: Si tienes problemas de HTTPS en Docker, podrías comentar esta línea temporalmente
app.UseHttpsRedirection();

app.MapControllers();

// Endpoint de prueba WeatherForecast
app.MapGet("/weatherforecast", () =>
{
    var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        )).ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

// INICIALIZACIÓN DE LA BASE DE DATOS
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Iniciando la migración de la base de datos...");
        await context.Database.MigrateAsync();
        logger.LogInformation("Migración completada exitosamente.");

        // CORRECCIÓN: Nombre estándar del método en DataSeeder
        await DataSeeder.SeedAsync(context); 
        logger.LogInformation("Datos iniciales cargados exitosamente.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error al inicializar la base de datos.");
        throw;
    }
}

await app.RunAsync();

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}