using System.Net;
using System.Net.Http.Json;
using Domus.Api.Features.Shared;
using Domus.Api.Features.Transactions.Shared;
using Domus.Api.Features.Users.Shared;
using FluentAssertions;

namespace Domus.Integration.Tests.Users;

[Collection("IntegrationTests")]
public class DeleteUserTests(DomusApiFactory factory) : IntegrationTestBase(factory)
{
    /// <summary>Verifica que a exclusão de um usuário existente retorna status 200.</summary>
    [Fact]
    public async Task DELETE_ExistingUser_Returns200()
    {
        var createResponse = await Client.PostAsJsonAsync(
            "/api/users",
            new { name = "Alice", age = 25 }
        );
        var created = await createResponse.Content.ReadFromJsonAsync<UserDto>();

        var deleteResponse = await Client.DeleteAsync($"/api/users/{created!.Id}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    /// <summary>Verifica que tentar excluir um usuário inexistente retorna status 404.</summary>
    [Fact]
    public async Task DELETE_NonExistentUser_Returns404()
    {
        var response = await Client.DeleteAsync($"/api/users/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>Verifica que ao excluir um usuário suas transações são removidas em cascata.</summary>
    [Fact]
    public async Task DELETE_UserWithTransactions_CascadeDeletesTransactions()
    {
        // Arrange: create user, category and transaction
        var userResponse = await Client.PostAsJsonAsync(
            "/api/users",
            new { name = "Alice", age = 25 }
        );
        var user = await userResponse.Content.ReadFromJsonAsync<UserDto>();

        var catResponse = await Client.PostAsJsonAsync(
            "/api/categories",
            new { description = "Food", finality = 1 }
        );
        var category = await catResponse.Content.ReadFromJsonAsync<dynamic>();
        var categoryId = ((System.Text.Json.JsonElement)category!).GetProperty("id").GetGuid();

        await Client.PostAsJsonAsync(
            "/api/transactions",
            new
            {
                description = "Lunch",
                value = 50.0m,
                type = 2, // Expense
                categoryId,
                userId = user!.Id,
            }
        );

        // Act: delete the user
        await Client.DeleteAsync($"/api/users/{user.Id}");

        // Assert: no transactions remain
        var txResponse = await Client.GetAsync($"/api/transactions?userId={user.Id}");
        var transactions = await txResponse.Content.ReadFromJsonAsync<
            PaginatedResult<TransactionDto>
        >();
        transactions!.totalItems.Should().Be(0);
    }
}
