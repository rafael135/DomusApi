using Domus.Core.Domain.Shared.Exceptions;
using Domus.Core.Domain.Transactions.Enums;

namespace Domus.Core.Domain.Transactions;

public class TransactionCategory : Entity
{
    public string Description { get; private set; }

    public TransactionCategoryType Finality { get; private set; }

    private readonly List<Transaction> _transactions = new();
    public virtual IReadOnlyCollection<Transaction> Transactions => this._transactions.AsReadOnly();

    protected TransactionCategory() { }

    public static TransactionCategory Create(string description, TransactionCategoryType finality)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new FormException(
                new Dictionary<string, string> { { "description", "Descrição inválida" } }
            );

        return new TransactionCategory
        {
            Id = Guid.NewGuid(),
            Description = description,
            Finality = finality,
        };
    }
}
