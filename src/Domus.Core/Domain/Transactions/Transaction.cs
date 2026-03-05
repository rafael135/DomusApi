using Domus.Core.Domain.Shared.Exceptions;
using Domus.Core.Domain.Transactions.Enums;
using Domus.Core.Domain.Transactions.Rules;
using Domus.Core.Domain.Users;

namespace Domus.Core.Domain.Transactions;

public class Transaction : Entity
{
    public string Description { get; private set; }
    public Decimal Value { get; private set; }
    public TransactionType Type { get; private set; }
    public Guid TransactionCategoryId { get; private set; }
    public virtual TransactionCategory TransactionCategory { get; private set; }

    public Guid UserId { get; private set; }
    public virtual User User { get; private set; }

    protected Transaction() { }

    public static Transaction Create(
        string description,
        decimal value,
        TransactionType type,
        TransactionCategory transactionCategory,
        User user
    )
    {
        CheckRule(new UserUnderageCannotRegisterIncomeRule(user, type));

        if (string.IsNullOrWhiteSpace(description))
            throw new FormException(
                new Dictionary<string, string> { { "description", "Descrição inválida" } }
            );

        if (value <= 0)
            throw new FormException(
                new Dictionary<string, string> { { "value", "Valor deve ser maior que zero" } }
            );

        if (
            (
                transactionCategory.Finality == TransactionCategoryType.Expense
                && type == TransactionType.Income
            )
            || (
                transactionCategory.Finality == TransactionCategoryType.Income
                && type == TransactionType.Expense
            )
        )
            throw new FormException(
                new Dictionary<string, string>
                {
                    { "type", "Categoria não é compatível com o tipo de transação" },
                }
            );

        return new Transaction
        {
            Id = Guid.NewGuid(),
            Description = description,
            Value = value,
            Type = type,
            TransactionCategoryId = transactionCategory.Id,
            UserId = user.Id,
            TransactionCategory = transactionCategory,
            User = user,
        };
    }
}
