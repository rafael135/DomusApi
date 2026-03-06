using Domus.Core.Domain.Shared.Exceptions;
using Domus.Core.Domain.Transactions.Enums;
using Domus.Core.Domain.Transactions.Rules;
using Domus.Core.Domain.Users;

namespace Domus.Core.Domain.Transactions;

/// <summary>
/// Representa uma transação financeira (receita ou despesa) associada a um usuário e a uma categoria.
/// </summary>
public class Transaction : Entity
{
    /// <summary>Descrição da transação.</summary>
    public string Description { get; private set; }
    /// <summary>Valor monetário da transação (deve ser maior que zero).</summary>
    public Decimal Value { get; private set; }
    /// <summary>Tipo da transação: receita ou despesa.</summary>
    public TransactionType Type { get; private set; }
    /// <summary>Identificador da categoria associada.</summary>
    public Guid TransactionCategoryId { get; private set; }
    /// <summary>Categoria associada à transação.</summary>
    public virtual TransactionCategory TransactionCategory { get; private set; }

    /// <summary>Identificador do usuário proprietário da transação.</summary>
    public Guid UserId { get; private set; }
    /// <summary>Usuário proprietário da transação.</summary>
    public virtual User User { get; private set; }

    /// <summary>
    /// Construtor protegido para uso pelo EF Core.
    /// </summary>
    protected Transaction() { }

    /// <summary>
    /// Cria uma nova transação aplicando validações de domínio e regras de negócio.
    /// </summary>
    /// <param name="description">Descrição da transação.</param>
    /// <param name="value">Valor da transação (deve ser maior que zero).</param>
    /// <param name="type">Tipo da transação (receita ou despesa).</param>
    /// <param name="transactionCategory">Categoria associada à transação.</param>
    /// <param name="user">Usuário responsável pela transação.</param>
    /// <returns>Nova instância de <see cref="Transaction"/>.</returns>
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
