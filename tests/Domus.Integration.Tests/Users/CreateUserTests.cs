using System.Net;
using System.Net.Http.Json;
using Domus.Api.Features.Users.Shared;
using FluentAssertions;

namespace Domus.Integration.Tests.Users;

[Collection("IntegrationTests")]
public class CreateUserTests(DomusApiFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task POST_ValidPayload_Returns200WithUser()
    {
        var response = await Client.PostAsJsonAsync("/api/users", new { name = "Alice", age = 25 });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var user = await response.Content.ReadFromJsonAsync<UserDto>();
        user!.Id.Should().NotBeEmpty();
        user.Name.Should().Be("Alice");
        user.Age.Should().Be(25);
    }

    [Fact]
    public async Task POST_EmptyName_Returns400()
    {
        var response = await Client.PostAsJsonAsync("/api/users", new { name = "", age = 25 });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(121)]
    public async Task POST_InvalidAge_Returns400(int age)
    {
        var response = await Client.PostAsJsonAsync("/api/users", new { name = "Bob", age });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
