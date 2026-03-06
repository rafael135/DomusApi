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

        /// <summary>Verifica que a criação com dados válidos (receita, categoria compatível) retorna uma transação corretamente inicializada.</summary>
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

        /// <summary>Verifica que valores zero ou negativos lançam <see cref="FormException"/> com o campo &quot;value&quot;.</summary>
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

        /// <summary>Verifica que descrição nula, vazia ou apenas espaços lança <see cref="FormException"/> com o campo &quot;description&quot;.</summary>
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

        /// <summary>Verifica que uma categoria de despesa não pode ser usada em transação de receita (e vice-versa), lançando <see cref="FormException"/> com o campo &quot;type&quot;.</summary>
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

        /// <summary>Verifica que um usuário menor de 18 anos não pode registrar transação de receita, lançando <see cref="BusinessRuleException"/>.</summary>
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
