using Domus.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Domus.Api.Features.Users.CreateUser;

/// <summary>
/// Endpoint para criação de um novo usuário. Rota: <c>POST /api/users</c>.
/// </summary>
public class CreateUserEndpoint : IEndpoint
{
    /// <summary>
    /// Mapeia a rota e o handler no <paramref name="app"/> fornecido.
    /// </summary>
    /// <param name="app">O <see cref="IEndpointRouteBuilder"/> onde a rota será registrada.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/users",
                async (IMediator mediator, [FromBody] CreateUserCommand command) =>
                {
                    var result = await mediator.Send(command);
                    return Results.Ok(result.User);
                }
            )
            .WithName("CreateUser")
            .WithTags("Users")
            .WithSummary("Create a new user.")
            .WithDescription("Creates a new user.")
            .Produces<CreateUserResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }
}
