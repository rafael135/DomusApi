using Domus.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace Domus.Integration.Tests;

/// <summary>
/// Fábrica da aplicação web utilizada nos testes de integração.
/// Sobe um container SQL Server isolado via Testcontainers e aplica as migrations antes dos testes.
/// </summary>
public class DomusApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    static DomusApiFactory()
    {
        var tempConfig = Path.Combine(Path.GetTempPath(), "testcontainers-docker-config");
        Directory.CreateDirectory(tempConfig);
        File.WriteAllText(Path.Combine(tempConfig, "config.json"), """{"auths":{}}""");
        Environment.SetEnvironmentVariable("DOCKER_CONFIG", tempConfig);
    }

    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2025-latest")
        .Build();

    /// <summary>
    /// Substitui o <see cref="DomusDbContext"/> registrado para apontar ao container de teste.
    /// </summary>
    /// <param name="builder">Construtor do host web de testes.</param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext registration
            var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<DomusDbContext>)
            );
            if (descriptor is not null)
                services.Remove(descriptor);

            // Register DbContext pointing to the test container
            services.AddDbContext<DomusDbContext>(options =>
                options.UseSqlServer(
                    _dbContainer.GetConnectionString(),
                    sql => sql.MigrationsAssembly("Domus.Infrastructure")
                )
            );
        });
    }

    /// <summary>
    /// Inicia o container SQL Server e aplica as migrations do banco de dados.
    /// </summary>
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        // Apply migrations so the schema is ready
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DomusDbContext>();
        await db.Database.MigrateAsync();
    }

    /// <summary>
    /// Para o container SQL Server ao final dos testes.
    /// </summary>
    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}
