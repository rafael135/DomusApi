using Domus.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Domus.Api.Features.Users.GetUsers;

/// <summary>
/// Endpoint para listagem paginada de usuários. Rota: <c>GET /api/users</c>.
/// </summary>
public class GetUsersEndpoint : IEndpoint
{
    /// <summary>
    /// Mapeia a rota e o handler no <paramref name="app"/> fornecido.
    /// </summary>
    /// <param name="app">O <see cref="IEndpointRouteBuilder"/> onde a rota será registrada.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/users",
                async (
                    IMediator mediator,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 10,
                    [FromQuery] string? searchTerm = null
                ) =>
                {
                    GetUsersQuery query = new GetUsersQuery(
                        pageNumber: pageNumber,
                        pageSize: pageSize,
                        searchTerm: searchTerm
                    );
                    var result = await mediator.Send(query);
                    return Results.Ok(result);
                }
            )
            .WithName("GetUsers")
            .WithTags("Users");
    }
}
