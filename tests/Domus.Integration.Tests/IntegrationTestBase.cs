using Domus.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Domus.Integration.Tests;

/// <summary>
/// Base class for all integration tests. Shares a single <see cref="DomusApiFactory"/>
/// instance (and therefore a single container) across all tests in the same class,
/// and cleans all tables between tests via <see cref="ResetDatabaseAsync"/>.
/// </summary>
public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected readonly HttpClient Client;
    private readonly DomusApiFactory _factory;

    protected IntegrationTestBase(DomusApiFactory factory)
    {
        _factory = factory;
        Client = factory.CreateClient();
    }

    public Task InitializeAsync() => ResetDatabaseAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    /// <summary>
    /// Deletes all rows from every table so each test starts with a clean database.
    /// Order respects FK constraints (child tables first).
    /// </summary>
    protected async Task ResetDatabaseAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DomusDbContext>();

        db.Transactions.RemoveRange(db.Transactions);
        db.TransactionCategories.RemoveRange(db.TransactionCategories);
        db.Users.RemoveRange(db.Users);
        await db.SaveChangesAsync();
    }
}
