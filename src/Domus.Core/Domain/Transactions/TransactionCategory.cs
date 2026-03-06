using Domus.Core.Domain.Shared.Exceptions;
using Domus.Core.Domain.Transactions.Enums;

namespace Domus.Core.Domain.Transactions;

/// <summary>
/// Representa uma categoria de transação financeira com uma finalidade específica (receita, despesa ou ambos).
/// </summary>
public class TransactionCategory : Entity
{
    /// <summary>Descrição da categoria (máximo de 400 caracteres).</summary>
    public string Description { get; private set; }

    /// <summary>Finalidade da categoria: despesa, receita ou ambos.</summary>
    public TransactionCategoryType Finality { get; private set; }

    private readonly List<Transaction> _transactions = new();
    /// <summary>Coleção de transações vinculadas a esta categoria.</summary>
    public virtual IReadOnlyCollection<Transaction> Transactions => this._transactions.AsReadOnly();

    /// <summary>
    /// Construtor protegido para uso pelo EF Core.
    /// </summary>
    protected TransactionCategory() { }

    /// <summary>
    /// Cria uma nova categoria de transação validando a descrição.
    /// </summary>
    /// <param name="description">Descrição da categoria.</param>
    /// <param name="finality">Finalidade da categoria.</param>
    /// <returns>Nova instância de <see cref="TransactionCategory"/>.</returns>
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
