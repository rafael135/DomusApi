using Domus.Core.Domain.Transactions;
using Domus.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Domus.Infrastructure.Database;

public class DomusDbContext : DbContext
{
    public DomusDbContext(DbContextOptions<DomusDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<TransactionCategory> TransactionCategories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
}
