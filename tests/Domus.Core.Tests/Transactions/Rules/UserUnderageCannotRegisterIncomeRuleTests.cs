using Domus.Core.Domain.Transactions.Enums;
using Domus.Core.Domain.Transactions.Rules;
using Domus.Core.Domain.Users;
using FluentAssertions;
using Xunit;

namespace Domus.Core.Tests.Transactions.Rules
{
    public class UserUnderageCannotRegisterIncomeRuleTests
    {
        /// <summary>Verifica que a regra é violada quando o usuário tem menos de 18 anos e o tipo é receita.</summary>
        [Fact]
        public void Rule_ShouldBeBroken_WhenUserUnder18AndTypeIncome()
        {
            var user = User.Create("Kid", 17);
            var rule = new UserUnderageCannotRegisterIncomeRule(user, TransactionType.Income);

            rule.IsBroken().Should().BeTrue();
            rule.Message.Should().Contain("menor de 18");
        }

        /// <summary>Verifica que a regra não é violada para outras combinações de idade e tipo de transação.</summary>
        [Theory]
        [InlineData(18, TransactionType.Income)]
        [InlineData(17, TransactionType.Expense)]
        [InlineData(20, TransactionType.Expense)]
        public void Rule_ShouldNotBeBroken_ForOtherSituations(int age, TransactionType type)
        {
            var user = User.Create("Person", age);
            var rule = new UserUnderageCannotRegisterIncomeRule(user, type);

            rule.IsBroken().Should().BeFalse();
        }
    }
}
