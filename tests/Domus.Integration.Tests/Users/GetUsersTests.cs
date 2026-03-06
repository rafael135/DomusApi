using System.Net;
using System.Net.Http.Json;
using Domus.Api.Features.Shared;
using Domus.Api.Features.Users.Shared;
using FluentAssertions;

namespace Domus.Integration.Tests.Users;

[Collection("IntegrationTests")]
public class GetUsersTests(DomusApiFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GET_NoUsers_ReturnsEmptyPage()
    {
        var response = await Client.GetAsync("/api/users");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResult<UserDto>>();
        result!.items.Should().BeEmpty();
        result.totalItems.Should().Be(0);
    }

    [Fact]
    public async Task GET_AfterCreation_ReturnsUser()
    {
        await Client.PostAsJsonAsync("/api/users", new { name = "Alice", age = 30 });
        await Client.PostAsJsonAsync("/api/users", new { name = "Bob", age = 25 });

        var response = await Client.GetAsync("/api/users?pageNumber=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PaginatedResult<UserDto>>();
        result!.totalItems.Should().Be(2);
        result.items.Should().ContainSingle(u => u.Name == "Alice");
        result.items.Should().ContainSingle(u => u.Name == "Bob");
    }

    [Fact]
    public async Task GET_WithSearchTerm_FiltersResults()
    {
        await Client.PostAsJsonAsync("/api/users", new { name = "Alice", age = 30 });
        await Client.PostAsJsonAsync("/api/users", new { name = "Bob", age = 25 });

        var response = await Client.GetAsync("/api/users?searchTerm=Alice");

        var result = await response.Content.ReadFromJsonAsync<PaginatedResult<UserDto>>();
        result!.totalItems.Should().Be(1);
        result.items.Should().ContainSingle(u => u.Name == "Alice");
    }
}
