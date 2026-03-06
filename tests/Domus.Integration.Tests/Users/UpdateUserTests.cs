using System.Net;
using System.Net.Http.Json;
using Domus.Api.Features.Users.Shared;
using FluentAssertions;

namespace Domus.Integration.Tests.Users;

[Collection("IntegrationTests")]
public class UpdateUserTests(DomusApiFactory factory) : IntegrationTestBase(factory)
{
    /// <summary>Verifica que a atualização de um usuário existente retorna status 200 com os dados atualizados.</summary>
    [Fact]
    public async Task PUT_ExistingUser_Returns200WithUpdatedData()
    {
        var createResponse = await Client.PostAsJsonAsync(
            "/api/users",
            new { name = "Alice", age = 25 }
        );
        var created = await createResponse.Content.ReadFromJsonAsync<UserDto>();

        var updateResponse = await Client.PutAsJsonAsync(
            $"/api/users/{created!.Id}",
            new { name = "Alice Updated", age = 26 }
        );

        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var updated = await updateResponse.Content.ReadFromJsonAsync<UserDto>();
        updated!.Name.Should().Be("Alice Updated");
        updated.Age.Should().Be(26);
        updated.Id.Should().Be(created.Id);
    }

    /// <summary>Verifica que tentar atualizar um usuário inexistente retorna status 404.</summary>
    [Fact]
    public async Task PUT_NonExistentUser_Returns404()
    {
        var response = await Client.PutAsJsonAsync(
            $"/api/users/{Guid.NewGuid()}",
            new { name = "Ghost", age = 30 }
        );

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>Verifica que dados inválidos (ex: nome vazio) retornam status 400.</summary>
    [Fact]
    public async Task PUT_InvalidData_Returns400()
    {
        var createResponse = await Client.PostAsJsonAsync(
            "/api/users",
            new { name = "Alice", age = 25 }
        );
        var created = await createResponse.Content.ReadFromJsonAsync<UserDto>();

        var response = await Client.PutAsJsonAsync(
            $"/api/users/{created!.Id}",
            new { name = "", age = 25 }
        );

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
