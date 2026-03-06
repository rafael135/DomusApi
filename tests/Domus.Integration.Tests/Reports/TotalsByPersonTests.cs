using System.Net;
using System.Net.Http.Json;
using Domus.Api.Features.Categories.Shared;
using Domus.Api.Features.Reports.TotalsByPerson;
using Domus.Api.Features.Users.Shared;
using FluentAssertions;

namespace Domus.Integration.Tests.Reports;

[Collection("IntegrationTests")]
public class TotalsByPersonTests(DomusApiFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GET_NoUsers_ReturnsEmptyListWithZeroTotals()
    {
        var response = await Client.GetAsync("/api/reports/totals-by-person");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetTotalsByPersonResult>();
        result!.Persons.Should().BeEmpty();
        result.TotalIncome.Should().Be(0);
        result.TotalExpense.Should().Be(0);
        result.NetBalance.Should().Be(0);
    }

    [Fact]
    public async Task GET_UserWithNoTransactions_AppearsWithZeroBalance()
    {
        await Client.PostAsJsonAsync("/api/users", new { name = "Alice", age = 30 });

        var response = await Client.GetAsync("/api/reports/totals-by-person");

        var result = await response.Content.ReadFromJsonAsync<GetTotalsByPersonResult>();
        result!.Persons.Should().ContainSingle();
        var person = result.Persons.First();
        person.TotalIncome.Should().Be(0);
        person.TotalExpense.Should().Be(0);
        person.Balance.Should().Be(0);
    }

    [Fact]
    public async Task GET_UserWithTransactions_CalculatesTotalsCorrectly()
    {
        // Create user and category
        var userResp = await Client.PostAsJsonAsync("/api/users", new { name = "Alice", age = 30 });
        var user = await userResp.Content.ReadFromJsonAsync<UserDto>();
        var catBothResp = await Client.PostAsJsonAsync(
            "/api/categories",
            new { description = "Mixed", finality = 3 }
        );
        var catBoth = await catBothResp.Content.ReadFromJsonAsync<CategoryDto>();

        // 2 incomes: 1000 + 500 = 1500; 1 expense: 300
        await Client.PostAsJsonAsync(
            "/api/transactions",
            new
            {
                description = "Salary",
                value = 1000m,
                type = 1,
                categoryId = catBoth!.Id,
                userId = user!.Id,
            }
        );
        await Client.PostAsJsonAsync(
            "/api/transactions",
            new
            {
                description = "Bonus",
                value = 500m,
                type = 1,
                categoryId = catBoth.Id,
                userId = user.Id,
            }
        );
        await Client.PostAsJsonAsync(
            "/api/transactions",
            new
            {
                description = "Rent",
                value = 300m,
                type = 2,
                categoryId = catBoth.Id,
                userId = user.Id,
            }
        );

        var response = await Client.GetAsync("/api/reports/totals-by-person");

        var result = await response.Content.ReadFromJsonAsync<GetTotalsByPersonResult>();
        result!.Persons.Should().ContainSingle();
        var person = result.Persons.First();
        person.TotalIncome.Should().Be(1500m);
        person.TotalExpense.Should().Be(300m);
        person.Balance.Should().Be(1200m);

        result.TotalIncome.Should().Be(1500m);
        result.TotalExpense.Should().Be(300m);
        result.NetBalance.Should().Be(1200m);
    }

    [Fact]
    public async Task GET_MultipleUsers_SumsTotalsCorrectly()
    {
        var user1Resp = await Client.PostAsJsonAsync(
            "/api/users",
            new { name = "Alice", age = 30 }
        );
        var user1 = await user1Resp.Content.ReadFromJsonAsync<UserDto>();
        var user2Resp = await Client.PostAsJsonAsync("/api/users", new { name = "Bob", age = 25 });
        var user2 = await user2Resp.Content.ReadFromJsonAsync<UserDto>();
        var catResp = await Client.PostAsJsonAsync(
            "/api/categories",
            new { description = "Mixed", finality = 3 }
        );
        var cat = await catResp.Content.ReadFromJsonAsync<CategoryDto>();

        await Client.PostAsJsonAsync(
            "/api/transactions",
            new
            {
                description = "Alice income",
                value = 1000m,
                type = 1,
                categoryId = cat!.Id,
                userId = user1!.Id,
            }
        );
        await Client.PostAsJsonAsync(
            "/api/transactions",
            new
            {
                description = "Bob expense",
                value = 200m,
                type = 2,
                categoryId = cat.Id,
                userId = user2!.Id,
            }
        );

        var response = await Client.GetAsync("/api/reports/totals-by-person");

        var result = await response.Content.ReadFromJsonAsync<GetTotalsByPersonResult>();
        result!.Persons.Should().HaveCount(2);
        result.TotalIncome.Should().Be(1000m);
        result.TotalExpense.Should().Be(200m);
        result.NetBalance.Should().Be(800m);
    }
}
