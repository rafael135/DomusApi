using Domus.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace Domus.Integration.Tests;

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

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        // Apply migrations so the schema is ready
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DomusDbContext>();
        await db.Database.MigrateAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}
