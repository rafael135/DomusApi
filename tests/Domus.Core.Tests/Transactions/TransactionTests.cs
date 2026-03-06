using Domus.Core.Domain.Shared.Exceptions;
using Domus.Core.Domain.Transactions;
using Domus.Core.Domain.Transactions.Enums;
using Domus.Core.Domain.Users;
using FluentAssertions;
using Xunit;

namespace Domus.Core.Tests.Transactions
{
    public class TransactionTests
    {
        private User CreateAdult() => User.Create("Adult", 30);

        private User CreateMinor() => User.Create("Minor", 16);

        private TransactionCategory CreateCategory(TransactionCategoryType finality) =>
            TransactionCategory.Create("Cat", finality);

        [Fact]
        public void Create_WithValidIncomeAndCompatibleCategory_ShouldSucceed()
        {
            var user = CreateAdult();
            var category = CreateCategory(TransactionCategoryType.Income);

            var tx = Transaction.Create("Salary", 1000m, TransactionType.Income, category, user);

            tx.Id.Should().NotBeEmpty();
            tx.Description.Should().Be("Salary");
            tx.Value.Should().Be(1000m);
            tx.Type.Should().Be(TransactionType.Income);
            tx.TransactionCategoryId.Should().Be(category.Id);
            tx.UserId.Should().Be(user.Id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void Create_WithNonPositiveValue_ShouldThrow(decimal value)
        {
            var user = CreateAdult();
            var category = CreateCategory(TransactionCategoryType.Expense);

            Action act = () =>
                Transaction.Create("desc", value, TransactionType.Expense, category, user);
            act.Should()
                .Throw<FormException>()
                .Where(e => ((FormException)e).Errors.ContainsKey("value"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithInvalidDescription_ShouldThrow(string desc)
        {
            var user = CreateAdult();
            var category = CreateCategory(TransactionCategoryType.Expense);

            Action act = () =>
                Transaction.Create(desc, 10, TransactionType.Expense, category, user);
            act.Should()
                .Throw<FormException>()
                .Where(e => ((FormException)e).Errors.ContainsKey("description"));
        }

        [Fact]
        public void Create_WithIncompatibleCategoryAndType_ShouldThrow()
        {
            var user = CreateAdult();
            var category = CreateCategory(TransactionCategoryType.Expense);

            Action act = () =>
                Transaction.Create("Incorrect", 50, TransactionType.Income, category, user);
            act.Should()
                .Throw<FormException>()
                .Where(e => ((FormException)e).Errors.ContainsKey("type"));

            var cat2 = CreateCategory(TransactionCategoryType.Income);
            Action act2 = () =>
                Transaction.Create("Incorrect", 50, TransactionType.Expense, cat2, user);
            act2.Should()
                .Throw<FormException>()
                .Where(e => ((FormException)e).Errors.ContainsKey("type"));
        }

        [Fact]
        public void Create_UnderageIncome_ShouldThrowBusinessRuleException()
        {
            var user = CreateMinor();
            var category = CreateCategory(TransactionCategoryType.Income);

            Action act = () =>
                Transaction.Create("Nope", 10, TransactionType.Income, category, user);
            act.Should().Throw<BusinessRuleException>();
        }
    }
}
