using Domus.Api.Extensions;
using Domus.Core.Domain.Transactions.Enums;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Domus.Api.Features.Categories.ListCategories;

/// <summary>
/// Endpoint para listagem paginada de categorias. Rota: <c>GET /api/categories</c>.
/// </summary>
public class ListCategoriesEndpoint : IEndpoint
{
    /// <summary>
    /// Mapeia a rota e o handler no <paramref name="app"/> fornecido.
    /// </summary>
    /// <param name="app">O <see cref="IEndpointRouteBuilder"/> onde a rota será registrada.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/categories",
                async (
                    IMediator mediator,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 10,
                    [FromQuery] TransactionCategoryType? finality = null
                ) =>
                {
                    var query = new ListCategoriesQuery(pageNumber, pageSize, finality);
                    var result = await mediator.Send(query);
                    return Results.Ok(result);
                }
            )
            .WithName("ListCategories")
            .WithTags("Categories");
    }
}
