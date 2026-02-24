using AuthService.Domain.Entitis; 
using AuthService.Domain.Constants;
using AuthService.Persistence.Data;
using AuthService.Application.Interfaces; 
using AuthService.Application.Services; 
using Microsoft.EntityFrameworkCore;

namespace AuthService.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuración de base de datos con Npgsql
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .UseSnakeCaseNamingConvention());

        // Registro de servicios de aplicación
        // Ahora el compilador encuentra EmailService en Application.Services
        services.AddScoped<IEmailService, EmailService>();

        services.AddHealthChecks();

        return services;
    }
}