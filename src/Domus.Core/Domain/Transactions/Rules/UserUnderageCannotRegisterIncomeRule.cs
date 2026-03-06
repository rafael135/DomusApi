using Domus.Core.Domain.Shared;
using Domus.Core.Domain.Users;

namespace Domus.Core.Domain.Transactions.Rules;

/// <summary>
/// Regra de negócio que impede usuários menores de 18 anos de registrarem transações de receita.
/// </summary>
public class UserUnderageCannotRegisterIncomeRule : IBusinessRule
{
    private readonly User _user;
    private readonly TransactionType _type;

    /// <summary>
    /// Inicializa a regra com o usuário e o tipo de transação a ser avaliado.
    /// </summary>
    /// <param name="user">Usuário que está registrando a transação.</param>
    /// <param name="type">Tipo da transação a ser validado.</param>
    public UserUnderageCannotRegisterIncomeRule(User user, TransactionType type)
    {
        _user = user;
        _type = type;
    }

    /// <summary>
    /// Retorna <c>true</c> se o usuário tiver menos de 18 anos e o tipo for <see cref="TransactionType.Income"/>.
    /// </summary>
    /// <returns><c>true</c> se a regra for violada; caso contrário, <c>false</c>.</returns>
    public bool IsBroken() => _user.Age < 18 && _type == TransactionType.Income;

    /// <summary>Mensagem descritiva da violação.</summary>
    public string Message => "Usuário menor de 18 anos não pode registrar transações de renda.";
}
