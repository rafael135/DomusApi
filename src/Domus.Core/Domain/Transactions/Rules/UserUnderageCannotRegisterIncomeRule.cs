using Domus.Core.Domain.Shared;
using Domus.Core.Domain.Users;

namespace Domus.Core.Domain.Transactions.Rules;

public class UserUnderageCannotRegisterIncomeRule : IBusinessRule
{
    private readonly User _user;
    private readonly TransactionType _type;

    public UserUnderageCannotRegisterIncomeRule(User user, TransactionType type)
    {
        _user = user;
        _type = type;
    }

    public bool IsBroken() => _user.Age < 18 && _type == TransactionType.Income;

    public string Message => "Usuário menor de 18 anos não pode registrar transações de renda.";
}
