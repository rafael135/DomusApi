using Domus.Core.Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
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
