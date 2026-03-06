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
        /// <summary>Verifica que a criação com dados válidos retorna um usuário com ID, nome, idade corretos e sem transações.</summary>
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

        /// <summary>Verifica que nome nulo, vazio ou apenas espaços lança <see cref="FormException"/> com o campo &quot;name&quot;.</summary>
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

        /// <summary>Verifica que idades fora do intervalo 0-120 lançam <see cref="FormException"/> com o campo &quot;age&quot;.</summary>
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

        /// <summary>Verifica que um usuário menor de 18 anos não pode registrar transação de receita, lançando <see cref="BusinessRuleException"/>.</summary>
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

        /// <summary>Verifica que o registro de uma despesa válida adiciona a transação à coleção do usuário.</summary>
        [Fact]
        public void RegisterTransaction_WithValidExpense_ShouldAddTransaction()
        {
            var user = User.Create("Alice", 20);
            var category = TransactionCategory.Create("Food", TransactionCategoryType.Expense);

            user.RegisterTransaction("lunch", 20, category, TransactionType.Expense);
            user.Transactions.Should().HaveCount(1);
            user.Transactions.First().Description.Should().Be("lunch");
        }

        /// <summary>Verifica que tentar remover uma transação que não pertence ao usuário lança <see cref="FormException"/> com o campo &quot;transaction&quot;.</summary>
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

        /// <summary>Verifica que a remoção de uma transação existente a elimina da coleção do usuário.</summary>
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
