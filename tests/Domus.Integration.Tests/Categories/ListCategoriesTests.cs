using System.Net;
using System.Net.Http.Json;
using Domus.Api.Features.Categories.Shared;
using Domus.Api.Features.Shared;
using Domus.Core.Domain.Transactions.Enums;
using FluentAssertions;

namespace Domus.Integration.Tests.Categories;

[Collection("IntegrationTests")]
public class ListCategoriesTests(DomusApiFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GET_NoCategories_ReturnsEmptyPage()
    {
        var response = await Client.GetAsync("/api/categories");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResult<CategoryDto>>();
        result!.items.Should().BeEmpty();
    }

    [Fact]
    public async Task GET_AfterCreation_ReturnsAllCategories()
    {
        await Client.PostAsJsonAsync("/api/categories", new { description = "Food", finality = 1 });
        await Client.PostAsJsonAsync(
            "/api/categories",
            new { description = "Salary", finality = 2 }
        );

        var response = await Client.GetAsync("/api/categories");

        var result = await response.Content.ReadFromJsonAsync<PaginatedResult<CategoryDto>>();
        result!.totalItems.Should().Be(2);
    }

    [Fact]
    public async Task GET_FilterByFinality_ReturnsOnlyMatchingCategories()
    {
        await Client.PostAsJsonAsync("/api/categories", new { description = "Food", finality = 1 }); // Expense
        await Client.PostAsJsonAsync(
            "/api/categories",
            new { description = "Salary", finality = 2 }
        ); // Income
        await Client.PostAsJsonAsync(
            "/api/categories",
            new { description = "Mixed", finality = 3 }
        ); // Both

        var response = await Client.GetAsync("/api/categories?finality=1");

        var result = await response.Content.ReadFromJsonAsync<PaginatedResult<CategoryDto>>();
        result!.totalItems.Should().Be(1);
        result.items.Should().ContainSingle(c => c.Description == "Food");
        result.items.All(c => c.Finality == TransactionCategoryType.Expense).Should().BeTrue();
    }
}
