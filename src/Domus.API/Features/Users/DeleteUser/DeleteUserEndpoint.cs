using Domus.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Domus.Api.Features.Users.DeleteUser;

/// <summary>
/// Endpoint para exclusão de um usuário. Rota: <c>DELETE /api/users/{userId}</c>.
/// </summary>
public class DeleteUserEndpoint : IEndpoint
{
    /// <summary>
    /// Mapeia a rota e o handler no <paramref name="app"/> fornecido.
    /// </summary>
    /// <param name="app">O <see cref="IEndpointRouteBuilder"/> onde a rota será registrada.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(
                "/api/users/{userId:guid}",
                async (IMediator mediator, Guid userId) =>
                {
                    var command = new DeleteUserCommand(userId);
                    var result = await mediator.Send(command);
                    return Results.Ok(result);
                }
            )
            .Produces<DeleteUserResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}
