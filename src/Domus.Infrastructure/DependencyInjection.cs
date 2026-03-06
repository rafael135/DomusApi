using Domus.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Domus.Infrastructure;

/// <summary>
/// Métodos de extensão para registro dos serviços de infraestrutura.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra os serviços de infraestrutura no contêiner de dependências.
    /// </summary>
    /// <param name="services">Coleção de serviços onde os serviços serão registrados.</param>
    /// <param name="configuration">Configuração da aplicação (appsettings).</param>
    /// <returns>A própria <see cref="IServiceCollection"/> para encadeamento de chamadas.</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddMemoryCache();

        string? connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found. " +
                "Ensure appsettings.json has ConnectionStrings:DefaultConnection and " +
                "ASPNETCORE_ENVIRONMENT is set correctly.");
        string? environment =
            configuration["ASPNETCORE_ENVIRONMENT"]
            ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? configuration["Environment"]
            ?? "Production";

        services.AddDbContext<DomusDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                options =>
                {
                    options.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null
                    );
                }
            )
        );

        services.AddHttpContextAccessor();

        return services;
    }
}
