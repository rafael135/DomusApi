using Domus.Core.Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domus.Infrastructure.Database.Configurations;

/// <summary>
/// Configuração de mapeamento EF Core para a entidade <see cref="TransactionCategory"/>.
/// </summary>
public class TransactionCategoryConfiguration : IEntityTypeConfiguration<TransactionCategory>
{
    /// <summary>
    /// Define as regras de mapeamento da entidade <see cref="TransactionCategory"/> para a tabela <c>TransactionCategories</c>.
    /// </summary>
    /// <param name="builder">Construtor de tipo utilizado para configurar a entidade.</param>
    public void Configure(EntityTypeBuilder<TransactionCategory> builder)
    {
        builder.ToTable("TransactionCategories");

        builder.Property(tc => tc.Description).HasMaxLength(400).IsRequired();

        builder.Property(tc => tc.Finality).IsRequired();

        builder
            .HasMany(tc => tc.Transactions)
            .WithOne(t => t.TransactionCategory)
            .HasForeignKey(t => t.TransactionCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
