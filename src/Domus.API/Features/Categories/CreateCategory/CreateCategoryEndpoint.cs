using Domus.Api.Extensions;
using Domus.Api.Features.Categories.Shared;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Domus.Api.Features.Categories.CreateCategory;

/// <summary>
/// Endpoint para criação de uma nova categoria. Rota: <c>POST /api/categories</c>.
/// </summary>
public class CreateCategoryEndpoint : IEndpoint
{
    /// <summary>
    /// Mapeia a rota e o handler no <paramref name="app"/> fornecido.
    /// </summary>
    /// <param name="app">O <see cref="IEndpointRouteBuilder"/> onde a rota será registrada.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/categories",
                async (IMediator mediator, [FromBody] CreateCategoryCommand command) =>
                {
                    var result = await mediator.Send(command);
                    return Results.Ok(result.Category);
                }
            )
            .WithName("CreateCategory")
            .WithTags("Categories")
            .WithSummary("Create a new category.")
            .Produces<CategoryDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }
}
