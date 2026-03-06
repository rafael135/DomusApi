using Domus.Core.Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Configuração de mapeamento EF Core para a entidade <see cref="Transaction"/>.
/// </summary>
public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    /// <summary>
    /// Define as regras de mapeamento da entidade <see cref="Transaction"/> para a tabela <c>Transactions</c>.
    /// </summary>
    /// <param name="builder">Construtor de tipo utilizado para configurar a entidade.</param>
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");

        builder.Property(t => t.Description).HasMaxLength(400).IsRequired();

        builder.Property(t => t.TransactionCategoryId).IsRequired();

        builder.Property(t => t.Value).HasColumnType("decimal(18,2)").IsRequired();

        builder.Property(t => t.Type).IsRequired();

        builder
            .HasOne(t => t.User)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(t => t.TransactionCategory)
            .WithMany(tc => tc.Transactions)
            .HasForeignKey(t => t.TransactionCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
