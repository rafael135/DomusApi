using System.Net;
using System.Net.Http.Json;
using Domus.Api.Features.Categories.Shared;
using Domus.Api.Features.Shared;
using Domus.Api.Features.Transactions.Shared;
using Domus.Api.Features.Users.Shared;
using FluentAssertions;

namespace Domus.Integration.Tests.Transactions;

[Collection("IntegrationTests")]
public class ListTransactionsTests(DomusApiFactory factory) : IntegrationTestBase(factory)
{
    /// <summary>Verifica que a listagem sem transações retorna uma página vazia com status 200.</summary>
    [Fact]
    public async Task GET_NoTransactions_ReturnsEmptyPage()
    {
        var response = await Client.GetAsync("/api/transactions");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResult<TransactionDto>>();
        result!.items.Should().BeEmpty();
    }

    /// <summary>Verifica que o filtro por userId retorna apenas as transações do usuário informado.</summary>
    [Fact]
    public async Task GET_FilterByUserId_ReturnsOnlyUserTransactions()
    {
        // Create two users and one category
        var user1Resp = await Client.PostAsJsonAsync(
            "/api/users",
            new { name = "Alice", age = 30 }
        );
        var user1 = await user1Resp.Content.ReadFromJsonAsync<UserDto>();
        var user2Resp = await Client.PostAsJsonAsync("/api/users", new { name = "Bob", age = 30 });
        var user2 = await user2Resp.Content.ReadFromJsonAsync<UserDto>();
        var catResp = await Client.PostAsJsonAsync(
            "/api/categories",
            new { description = "Food", finality = 1 }
        );
        var cat = await catResp.Content.ReadFromJsonAsync<CategoryDto>();

        await Client.PostAsJsonAsync(
            "/api/transactions",
            new
            {
                description = "Alice tx",
                value = 10m,
                type = 2,
                categoryId = cat!.Id,
                userId = user1!.Id,
            }
        );
        await Client.PostAsJsonAsync(
            "/api/transactions",
            new
            {
                description = "Bob tx",
                value = 20m,
                type = 2,
                categoryId = cat.Id,
                userId = user2!.Id,
            }
        );

        var response = await Client.GetAsync($"/api/transactions?userId={user1.Id}");

        var result = await response.Content.ReadFromJsonAsync<PaginatedResult<TransactionDto>>();
        result!.totalItems.Should().Be(1);
        result.items.Should().ContainSingle(t => t.Description == "Alice tx");
    }

    /// <summary>Verifica que o filtro por categoryId retorna apenas as transações da categoria informada.</summary>
    [Fact]
    public async Task GET_FilterByCategoryId_ReturnsOnlyCategoryTransactions()
    {
        var userResp = await Client.PostAsJsonAsync("/api/users", new { name = "Alice", age = 30 });
        var user = await userResp.Content.ReadFromJsonAsync<UserDto>();
        var cat1Resp = await Client.PostAsJsonAsync(
            "/api/categories",
            new { description = "Food", finality = 1 }
        );
        var cat1 = await cat1Resp.Content.ReadFromJsonAsync<CategoryDto>();
        var cat2Resp = await Client.PostAsJsonAsync(
            "/api/categories",
            new { description = "Transport", finality = 1 }
        );
        var cat2 = await cat2Resp.Content.ReadFromJsonAsync<CategoryDto>();

        await Client.PostAsJsonAsync(
            "/api/transactions",
            new
            {
                description = "Lunch",
                value = 30m,
                type = 2,
                categoryId = cat1!.Id,
                userId = user!.Id,
            }
        );
        await Client.PostAsJsonAsync(
            "/api/transactions",
            new
            {
                description = "Bus",
                value = 5m,
                type = 2,
                categoryId = cat2!.Id,
                userId = user.Id,
            }
        );

        var response = await Client.GetAsync($"/api/transactions?categoryId={cat1.Id}");

        var result = await response.Content.ReadFromJsonAsync<PaginatedResult<TransactionDto>>();
        result!.totalItems.Should().Be(1);
        result.items.Should().ContainSingle(t => t.Description == "Lunch");
    }
}
