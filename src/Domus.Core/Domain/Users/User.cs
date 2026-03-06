using Domus.Core;
using Domus.Core.Domain.Shared.Exceptions;
using Domus.Core.Domain.Transactions;
using Domus.Core.Domain.Transactions.Enums;
using Domus.Core.Domain.Transactions.Rules;

namespace Domus.Core.Domain.Users;

/// <summary>
/// Representa um usuário do sistema de finanças pessoais.
/// </summary>
public class User : Entity
{
    /// <summary>Nome do usuário (máximo de 200 caracteres).</summary>
    public string Name { get; private set; }

    /// <summary>Idade do usuário (entre 0 e 120 anos).</summary>
    public int Age { get; private set; }

    private readonly List<Transaction> _transactions = new();
    /// <summary>Coleção de transações registradas pelo usuário.</summary>
    public virtual IReadOnlyCollection<Transaction> Transactions => this._transactions.AsReadOnly();

    /// <summary>
    /// Construtor protegido para uso pelo EF Core na materialização de objetos.
    /// </summary>
    protected User() { }

    /// <summary>
    /// Cria um novo usuário validando nome e idade.
    /// </summary>
    /// <param name="name">Nome do usuário (não pode ser vazio).</param>
    /// <param name="age">Idade do usuário (entre 0 e 120).</param>
    /// <returns>Nova instância de <see cref="User"/>.</returns>
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

    /// <summary>
    /// Atualiza os dados do usuário validando nome e idade.
    /// </summary>
    /// <param name="name">Novo nome do usuário.</param>
    /// <param name="age">Nova idade do usuário.</param>
    public void Update(string name, int age)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new FormException(new Dictionary<string, string> { { "name", "Nome inválido" } });

        if (age < 0 || age > 120)
            throw new FormException(new Dictionary<string, string> { { "age", "Idade inválida" } });

        Name = name;
        Age = age;
    }

    /// <summary>
    /// Cria e registra uma nova transação para o usuário.
    /// </summary>
    /// <param name="description">Descrição da transação.</param>
    /// <param name="value">Valor da transação (deve ser positivo).</param>
    /// <param name="category">Categoria associada à transação.</param>
    /// <param name="type">Tipo da transação (<see cref="TransactionType.Income"/> ou <see cref="TransactionType.Expense"/>).</param>
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

    /// <summary>
    /// Remove uma transação previamente registrada pelo usuário.
    /// </summary>
    /// <param name="transaction">A transação a ser removida.</param>
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
