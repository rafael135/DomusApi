namespace Domus.Core.Domain.Transactions.Enums;

/// <summary>
/// Define a finalidade de uma categoria de transação.
/// </summary>
public enum TransactionCategoryType
{
    /// <summary>Categoria destinada a despesas.</summary>
    Expense = 1,
    /// <summary>Categoria destinada a receitas.</summary>
    Income = 2,
    /// <summary>Categoria compatível com despesas e receitas.</summary>
    Both = 3,
}
