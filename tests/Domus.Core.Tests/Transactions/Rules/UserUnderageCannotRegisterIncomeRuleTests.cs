using FluentAssertions;
using Xunit;
using Domus.Core.Domain.Transactions.Rules;
using Domus.Core.Domain.Users;
using Domus.Core.Domain.Transactions.Enums;

namespace Domus.Core.Tests.Transactions.Rules
{
    public class UserUnderageCannotRegisterIncomeRuleTests
    {
        [Fact]
        public void Rule_ShouldBeBroken_WhenUserUnder18AndTypeIncome()
        {
            var user = User.Create("Kid", 17);
            var rule = new UserUnderageCannotRegisterIncomeRule(user, TransactionType.Income);

            rule.IsBroken().Should().BeTrue();
            rule.Message.Should().Contain("menor de 18");
        }

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