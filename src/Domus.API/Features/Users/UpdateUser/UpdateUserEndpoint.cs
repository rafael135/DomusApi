using Domus.Api.Extensions;
using Domus.Api.Features.Users.Shared;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Domus.Api.Features.Users.UpdateUser;

/// <summary>
/// Endpoint para atualização de um usuário. Rota: <c>PUT /api/users/{userId}</c>.
/// </summary>
public class UpdateUserEndpoint : IEndpoint
{
    /// <summary>
    /// Mapeia a rota e o handler no <paramref name="app"/> fornecido.
    /// </summary>
    /// <param name="app">O <see cref="IEndpointRouteBuilder"/> onde a rota será registrada.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(
                "/api/users/{userId:guid}",
                async (IMediator mediator, Guid userId, [FromBody] UpdateUserBody body) =>
                {
                    var command = new UpdateUserCommand(userId, body.Name, body.Age);
                    var result = await mediator.Send(command);
                    return Results.Ok(result.User);
                }
            )
            .WithName("UpdateUser")
            .WithTags("Users")
            .WithSummary("Update an existing user.")
            .Produces<UserDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }
}

/// <summary>
/// Corpo da requisição de atualização de usuário.
/// </summary>
/// <param name="Name">Novo nome do usuário.</param>
/// <param name="Age">Nova idade do usuário.</param>
public record UpdateUserBody(string Name, int Age);
