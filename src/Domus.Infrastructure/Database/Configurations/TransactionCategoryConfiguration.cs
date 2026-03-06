using Domus.Core.Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domus.Infrastructure.Database.Configurations;

public class TransactionCategoryConfiguration : IEntityTypeConfiguration<TransactionCategory>
{
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
