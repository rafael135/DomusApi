using Domus.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Domus.Infrastructure.Database;

/// <summary>
/// DomusDbContextFactory is used by EF Core tools at design time to create
/// instances of DomusDbContext for things like migrations.
/// </summary>
public class DomusDbContextFactory : IDesignTimeDbContextFactory<DomusDbContext>
{
    /// <summary>
    /// Creates a <see cref="DomusDbContext"/> instance for design-time tools such as migrations.
    /// The factory reads configuration files from the API project folder to obtain the connection string.
    /// </summary>
    /// <param name="args">Design-time arguments (not used).</param>
    /// <returns>An initialized <see cref="DomusDbContext"/>.</returns>
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
