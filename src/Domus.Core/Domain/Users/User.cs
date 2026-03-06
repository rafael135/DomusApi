using Domus.Core;
using Domus.Core.Domain.Shared.Exceptions;
using Domus.Core.Domain.Transactions;
using Domus.Core.Domain.Transactions.Enums;
using Domus.Core.Domain.Transactions.Rules;

namespace Domus.Core.Domain.Users;

public class User : Entity
{
    public string Name { get; private set; }

    public int Age { get; private set; }

    private readonly List<Transaction> _transactions = new();
    public virtual IReadOnlyCollection<Transaction> Transactions => this._transactions.AsReadOnly();

    /// <summary>
    /// Protected constructor for EF Core. This allows EF Core to create instances of User when materializing data from the database.
    /// </summary>
    protected User() { }

    public static User Create(string name, int age)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new FormException(new Dictionary<string, string> { { "name", "Nome inválido" } });

        if (age < 0 || age > 120)
            throw new FormException(new Dictionary<string, string> { { "age", "Idade inválida" } });

        return new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Age = age,
        };
    }

    public void Update(string name, int age)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new FormException(new Dictionary<string, string> { { "name", "Nome inválido" } });

        if (age < 0 || age > 120)
            throw new FormException(new Dictionary<string, string> { { "age", "Idade inválida" } });

        Name = name;
        Age = age;
    }

    public void RegisterTransaction(
        string description,
        decimal value,
        TransactionCategory category,
        TransactionType type
    )
    {
        Transaction transaction = Transaction.Create(description, value, type, category, this);
        _transactions.Add(transaction);
    }

    public void RemoveTransaction(Transaction transaction)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));

        if (!_transactions.Contains(transaction))
            throw new FormException(
                new Dictionary<string, string>
                {
                    { "transaction", "Transação não pertence a este usuário" },
                }
            );

        _transactions.Remove(transaction);
    }
}
