using System.Net;
using System.Net.Http.Json;
using Domus.Api.Features.Categories.Shared;
using Domus.Api.Features.Reports.TotalsByCategory;
using Domus.Api.Features.Users.Shared;
using FluentAssertions;

namespace Domus.Integration.Tests.Reports;

[Collection("IntegrationTests")]
public class TotalsByCategoryTests(DomusApiFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GET_NoCategories_ReturnsEmptyListWithZeroTotals()
    {
        var response = await Client.GetAsync("/api/reports/totals-by-category");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetTotalsByCategoryResult>();
        result!.Categories.Should().BeEmpty();
        result.TotalIncome.Should().Be(0);
        result.TotalExpense.Should().Be(0);
        result.NetBalance.Should().Be(0);
    }

    [Fact]
    public async Task GET_CategoryWithNoTransactions_AppearsWithZeroBalance()
    {
        await Client.PostAsJsonAsync("/api/categories", new { description = "Food", finality = 1 });

        var response = await Client.GetAsync("/api/reports/totals-by-category");

        var result = await response.Content.ReadFromJsonAsync<GetTotalsByCategoryResult>();
        result!.Categories.Should().ContainSingle();
        var cat = result.Categories.First();
        cat.TotalIncome.Should().Be(0);
        cat.TotalExpense.Should().Be(0);
        cat.Balance.Should().Be(0);
    }

    [Fact]
    public async Task GET_CategoryWithTransactions_CalculatesTotalsCorrectly()
    {
        var userResp = await Client.PostAsJsonAsync("/api/users", new { name = "Alice", age = 30 });
        var user = await userResp.Content.ReadFromJsonAsync<UserDto>();
        var catResp = await Client.PostAsJsonAsync("/api/categories", new { description = "Mixed", finality = 3 });
        var cat = await catResp.Content.ReadFromJsonAsync<CategoryDto>();

        await Client.PostAsJsonAsync("/api/transactions",
            new { description = "Income 1", value = 800m, type = 1, categoryId = cat!.Id, userId = user!.Id });
        await Client.PostAsJsonAsync("/api/transactions",
            new { description = "Expense 1", value = 250m, type = 2, categoryId = cat.Id, userId = user.Id });

        var response = await Client.GetAsync("/api/reports/totals-by-category");

        var result = await response.Content.ReadFromJsonAsync<GetTotalsByCategoryResult>();
        result!.Categories.Should().ContainSingle();
        var category = result.Categories.First();
        category.TotalIncome.Should().Be(800m);
        category.TotalExpense.Should().Be(250m);
        category.Balance.Should().Be(550m);

        result.TotalIncome.Should().Be(800m);
        result.TotalExpense.Should().Be(250m);
        result.NetBalance.Should().Be(550m);
    }

    [Fact]
    public async Task GET_MultipleCategories_SumsTotalsCorrectly()
    {
        var userResp = await Client.PostAsJsonAsync("/api/users", new { name = "Alice", age = 30 });
        var user = await userResp.Content.ReadFromJsonAsync<UserDto>();
        var cat1Resp = await Client.PostAsJsonAsync("/api/categories", new { description = "Food", finality = 1 });
        var cat1 = await cat1Resp.Content.ReadFromJsonAsync<CategoryDto>();
        var cat2Resp = await Client.PostAsJsonAsync("/api/categories", new { description = "Salary", finality = 2 });
        var cat2 = await cat2Resp.Content.ReadFromJsonAsync<CategoryDto>();

        await Client.PostAsJsonAsync("/api/transactions",
            new { description = "Lunch", value = 100m, type = 2, categoryId = cat1!.Id, userId = user!.Id });
        await Client.PostAsJsonAsync("/api/transactions",
            new { description = "Paycheck", value = 2000m, type = 1, categoryId = cat2!.Id, userId = user.Id });

        var response = await Client.GetAsync("/api/reports/totals-by-category");

        var result = await response.Content.ReadFromJsonAsync<GetTotalsByCategoryResult>();
        result!.Categories.Should().HaveCount(2);
        result.TotalIncome.Should().Be(2000m);
        result.TotalExpense.Should().Be(100m);
        result.NetBalance.Should().Be(1900m);
    }
}
