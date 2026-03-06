using System.Linq;
using Domus.Core.Domain.Shared.Exceptions;
using Domus.Core.Domain.Transactions;
using Domus.Core.Domain.Transactions;
using Domus.Core.Domain.Transactions.Enums;
using Domus.Core.Domain.Users;
using FluentAssertions;
using Xunit;

namespace Domus.Core.Tests.Users
{
    public class UserTests
    {
        [Fact]
        public void Create_WithValidData_ShouldReturnUser()
        {
            // arrange
            string name = "Alice";
            int age = 30;

            // act
            User user = User.Create(name, age);

            // assert
            user.Id.Should().NotBeEmpty();
            user.Name.Should().Be(name);
            user.Age.Should().Be(age);
            user.Transactions.Should().BeEmpty();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithInvalidName_ShouldThrow(string invalid)
        {
            Action act = () => User.Create(invalid, 25);
            act.Should()
                .Throw<FormException>()
                .Where(e => ((FormException)e).Errors.ContainsKey("name"));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(121)]
        public void Create_WithInvalidAge_ShouldThrow(int invalidAge)
        {
            Action act = () => User.Create("Bob", invalidAge);
            act.Should()
                .Throw<FormException>()
                .Where(e => ((FormException)e).Errors.ContainsKey("age"));
        }

        [Fact]
        public void RegisterTransaction_WithUnderageIncome_ShouldThrowBusinessRuleException()
        {
            var user = User.Create("Kid", 16);
            var category = TransactionCategory.Create("Salary", TransactionCategoryType.Income);

            Action act = () =>
                user.RegisterTransaction("salary", 100, category, TransactionType.Income);
            act.Should().Throw<BusinessRuleException>();
            user.Transactions.Should().BeEmpty();
        }

        [Fact]
        public void RegisterTransaction_WithValidExpense_ShouldAddTransaction()
        {
            var user = User.Create("Alice", 20);
            var category = TransactionCategory.Create("Food", TransactionCategoryType.Expense);

            user.RegisterTransaction("lunch", 20, category, TransactionType.Expense);
            user.Transactions.Should().HaveCount(1);
            user.Transactions.First().Description.Should().Be("lunch");
        }

        [Fact]
        public void RemoveTransaction_NotInUser_ShouldThrow()
        {
            var user = User.Create("Alice", 20);
            var otherUser = User.Create("Bob", 25);
            var category = TransactionCategory.Create("Food", TransactionCategoryType.Expense);
            var tx = Transaction.Create("t", 10, TransactionType.Expense, category, otherUser);

            Action act = () => user.RemoveTransaction(tx);
            act.Should()
                .Throw<FormException>()
                .Where(e => ((FormException)e).Errors.ContainsKey("transaction"));
        }

        [Fact]
        public void RemoveTransaction_WithExistingTransaction_ShouldRemove()
        {
            var user = User.Create("Alice", 20);
            var category = TransactionCategory.Create("Food", TransactionCategoryType.Expense);
            user.RegisterTransaction("t", 10, category, TransactionType.Expense);
            var tx = user.Transactions.First();

            user.RemoveTransaction(tx);
            user.Transactions.Should().BeEmpty();
        }
    }
}
