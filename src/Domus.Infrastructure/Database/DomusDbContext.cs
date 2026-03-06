using Domus.Core.Domain.Transactions;
using Domus.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Domus.Infrastructure.Database;

/// <summary>
/// Contexto principal do banco de dados da aplicação Domus, baseado no Entity Framework Core.
/// </summary>
public class DomusDbContext : DbContext
{
    /// <summary>
    /// Inicializa o contexto com as opções configuradas.
    /// </summary>
    /// <param name="options">Opções de configuração do contexto.</param>
    public DomusDbContext(DbContextOptions<DomusDbContext> options)
        : base(options) { }

    /// <summary>Tabela de usuários.</summary>
    public DbSet<User> Users { get; set; }
    /// <summary>Tabela de categorias de transações.</summary>
    public DbSet<TransactionCategory> TransactionCategories { get; set; }
    /// <summary>Tabela de transações financeiras.</summary>
    public DbSet<Transaction> Transactions { get; set; }

    /// <summary>
    /// Aplica todas as configurações de mapeamento (<see cref="IEntityTypeConfiguration{T}"/>) do assembly de infraestrutura.
    /// </summary>
    /// <param name="modelBuilder">Construtor do modelo EF Core.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DomusDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
