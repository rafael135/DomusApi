using Domus.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Domus.Infrastructure.Database;

/// <summary>
/// Fábrica de <see cref="DomusDbContext"/> utilizada pelas ferramentas de design-time do EF Core (ex: migrations).
/// </summary>
public class DomusDbContextFactory : IDesignTimeDbContextFactory<DomusDbContext>
{
    /// <summary>
    /// Cria uma instância de <see cref="DomusDbContext"/> lendo a string de conexão dos arquivos de configuração do projeto de API.
    /// </summary>
    /// <param name="args">Argumentos de design-time (não utilizados).</param>
    /// <returns>Instância configurada de <see cref="DomusDbContext"/>.</returns>
    public DomusDbContext CreateDbContext(string[] args)
    {
        // 1. Configures where to read the connection string
        // We need to point to the API folder to read the appsettings.json from there
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../Domus.Api");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        // 2. Configure the Database Builder
        var builder = new DbContextOptionsBuilder<DomusDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Domus.Infrastructure"));

        // 3. Return the Context
        return new DomusDbContext(builder.Options);
    }
}
