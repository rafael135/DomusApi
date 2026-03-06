using System.Net;
using System.Net.Http.Json;
using Domus.Api.Features.Categories.Shared;
using FluentAssertions;

namespace Domus.Integration.Tests.Categories;

[Collection("IntegrationTests")]
public class CreateCategoryTests(DomusApiFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task POST_ValidExpenseCategory_Returns200()
    {
        var response = await Client.PostAsJsonAsync("/api/categories",
            new { description = "Food", finality = 1 }); // 1 = Expense

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var category = await response.Content.ReadFromJsonAsync<CategoryDto>();
        category!.Id.Should().NotBeEmpty();
        category.Description.Should().Be("Food");
        category.Finality.Should().Be(Domus.Core.Domain.Transactions.Enums.TransactionCategoryType.Expense);
    }

    [Fact]
    public async Task POST_EmptyDescription_Returns400()
    {
        var response = await Client.PostAsJsonAsync("/api/categories",
            new { description = "", finality = 1 });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(1)] // Expense
    [InlineData(2)] // Income
    [InlineData(3)] // Both
    public async Task POST_AllFinalityTypes_Return200(int finality)
    {
        var response = await Client.PostAsJsonAsync("/api/categories",
            new { description = $"Category finality {finality}", finality });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
