using Domus.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domus.Infrastructure.Database.Configurations;

/// <summary>
/// Configuração de mapeamento EF Core para a entidade <see cref="User"/>.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Define as regras de mapeamento da entidade <see cref="User"/> para a tabela <c>Users</c>.
    /// </summary>
    /// <param name="builder">Construtor de tipo utilizado para configurar a entidade.</param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Property(u => u.Name).HasMaxLength(200).IsRequired();

        builder.Property(u => u.Age).IsRequired();

        builder
            .HasMany(u => u.Transactions)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
