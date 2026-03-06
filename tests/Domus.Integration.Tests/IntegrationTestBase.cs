using Domus.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Domus.Integration.Tests;

/// <summary>
/// Classe base para todos os testes de integração. Compartilha uma única instância de <see cref="DomusApiFactory"/>
/// (e portanto um único container) entre todos os testes da mesma sessão, limpando as tabelas entre cada teste.
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
    /// Remove todos os registros de todas as tabelas para garantir isolamento entre testes.
    /// A ordem de exclusão respeita as restrições de chave estrangeira (tabelas filhas primeiro).
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
