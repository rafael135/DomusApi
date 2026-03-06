using System.Net;
using System.Net.Http.Json;
using Domus.Api.Features.Categories.Shared;
using Domus.Api.Features.Transactions.Shared;
using Domus.Api.Features.Users.Shared;
using FluentAssertions;

namespace Domus.Integration.Tests.Transactions;

[Collection("IntegrationTests")]
public class CreateTransactionTests(DomusApiFactory factory) : IntegrationTestBase(factory)
{
    private async Task<(Guid userId, Guid categoryId)> CreatePrerequisitesAsync(
        int userAge = 30,
        int categoryFinality = 1
    )
    {
        var userResp = await Client.PostAsJsonAsync(
            "/api/users",
            new { name = "Alice", age = userAge }
        );
        var user = await userResp.Content.ReadFromJsonAsync<UserDto>();

        var catResp = await Client.PostAsJsonAsync(
            "/api/categories",
            new { description = "Category", finality = categoryFinality }
        );
        var cat = await catResp.Content.ReadFromJsonAsync<CategoryDto>();

        return (user!.Id, cat!.Id);
    }

    /// <summary>Verifica que uma transação de despesa válida retorna status 200.</summary>
    [Fact]
    public async Task POST_ValidExpenseTransaction_Returns200()
    {
        var (userId, categoryId) = await CreatePrerequisitesAsync(userAge: 30, categoryFinality: 1);

        var response = await Client.PostAsJsonAsync(
            "/api/transactions",
            new
            {
                description = "Lunch",
                value = 50.0m,
                type = 2, // Expense
                categoryId,
                userId,
            }
        );

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var tx = await response.Content.ReadFromJsonAsync<TransactionDto>();
        tx!.Id.Should().NotBeEmpty();
        tx.Description.Should().Be("Lunch");
        tx.Value.Should().Be(50.0m);
    }

    /// <summary>Verifica que uma transação de renda para usuário adulto retorna status 200.</summary>
    [Fact]
    public async Task POST_ValidIncomeTransactionForAdult_Returns200()
    {
        var (userId, categoryId) = await CreatePrerequisitesAsync(userAge: 20, categoryFinality: 2); // Income category

        var response = await Client.PostAsJsonAsync(
            "/api/transactions",
            new
            {
                description = "Salary",
                value = 3000.0m,
                type = 1, // Income
                categoryId,
                userId,
            }
        );

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    /// <summary>Verifica que usuário menor de idade não pode registrar renda, retornando status 422.</summary>
    [Fact]
    public async Task POST_UnderageUserWithIncomeType_Returns422()
    {
        var (userId, categoryId) = await CreatePrerequisitesAsync(userAge: 16, categoryFinality: 2); // Income category

        var response = await Client.PostAsJsonAsync(
            "/api/transactions",
            new
            {
                description = "Salary",
                value = 1000.0m,
                type = 1, // Income
                categoryId,
                userId,
            }
        );

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    /// <summary>Verifica que categoria incompatível com o tipo da transação retorna status 400.</summary>
    [Fact]
    public async Task POST_IncompatibleCategoryFinality_Returns400()
    {
        // Expense category used with Income transaction type
        var (userId, categoryId) = await CreatePrerequisitesAsync(userAge: 30, categoryFinality: 1);

        var response = await Client.PostAsJsonAsync(
            "/api/transactions",
            new
            {
                description = "Salary",
                value = 1000.0m,
                type = 1, // Income
                categoryId,
                userId,
            }
        );

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    /// <summary>Verifica que categoria com finalidade Both aceita qualquer tipo de transação.</summary>
    [Fact]
    public async Task POST_CategoryWithBothFinality_AcceptsAnyType()
    {
        var (userId, categoryId) = await CreatePrerequisitesAsync(userAge: 25, categoryFinality: 3); // Both

        var expenseResponse = await Client.PostAsJsonAsync(
            "/api/transactions",
            new
            {
                description = "Expense",
                value = 100.0m,
                type = 2, // Expense
                categoryId,
                userId,
            }
        );

        var incomeResponse = await Client.PostAsJsonAsync(
            "/api/transactions",
            new
            {
                description = "Income",
                value = 200.0m,
                type = 1, // Income
                categoryId,
                userId,
            }
        );

        expenseResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        incomeResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    /// <summary>Verifica que usuário inexistente retorna status 404.</summary>
    [Fact]
    public async Task POST_NonExistentUser_Returns404()
    {
        var catResp = await Client.PostAsJsonAsync(
            "/api/categories",
            new { description = "Category", finality = 1 }
        );
        var cat = await catResp.Content.ReadFromJsonAsync<CategoryDto>();

        var response = await Client.PostAsJsonAsync(
            "/api/transactions",
            new
            {
                description = "Test",
                value = 10.0m,
                type = 2,
                categoryId = cat!.Id,
                userId = Guid.NewGuid(),
            }
        );

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>Verifica que categoria inexistente retorna status 404.</summary>
    [Fact]
    public async Task POST_NonExistentCategory_Returns404()
    {
        var userResp = await Client.PostAsJsonAsync("/api/users", new { name = "Alice", age = 25 });
        var user = await userResp.Content.ReadFromJsonAsync<UserDto>();

        var response = await Client.PostAsJsonAsync(
            "/api/transactions",
            new
            {
                description = "Test",
                value = 10.0m,
                type = 2,
                categoryId = Guid.NewGuid(),
                userId = user!.Id,
            }
        );

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>Verifica que valores não positivos (zero ou negativo) retornam status 400.</summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task POST_NonPositiveValue_Returns400(decimal value)
    {
        var (userId, categoryId) = await CreatePrerequisitesAsync();

        var response = await Client.PostAsJsonAsync(
            "/api/transactions",
            new
            {
                description = "Test",
                value,
                type = 2,
                categoryId,
                userId,
            }
        );

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
