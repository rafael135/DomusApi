using Domus.Core.Domain.Shared.Exceptions;
using Domus.Core.Domain.Transactions;
using Domus.Core.Domain.Transactions.Enums;
using FluentAssertions;
using Xunit;

namespace Domus.Core.Tests.Categories
{
    public class TransactionCategoryTests
    {
        /// <summary>Verifica que a criação com dados válidos retorna uma categoria com ID, descrição e finalidade corretos.</summary>
        [Fact]
        public void Create_WithValidData_ShouldSucceed()
        {
            var cat = TransactionCategory.Create("Food", TransactionCategoryType.Expense);
            cat.Id.Should().NotBeEmpty();
            cat.Description.Should().Be("Food");
            cat.Finality.Should().Be(TransactionCategoryType.Expense);
        }

        /// <summary>Verifica que a criação com descrição nula, vazia ou apenas espaços lança <see cref="FormException"/> com o campo &quot;description&quot;.</summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithInvalidDescription_ShouldThrow(string desc)
        {
            Action act = () => TransactionCategory.Create(desc, TransactionCategoryType.Income);
            act.Should()
                .Throw<FormException>()
                .Where(e => ((FormException)e).Errors.ContainsKey("description"));
        }
    }
}
