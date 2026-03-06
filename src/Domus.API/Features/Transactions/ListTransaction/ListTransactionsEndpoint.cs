using Domus.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Domus.Api.Features.Transactions.ListTransaction;

/// <summary>
/// Endpoint para listagem paginada de transações. Rota: <c>GET /api/transactions</c>.
/// </summary>
public class ListTransactionsEndpoint : IEndpoint
{
    /// <summary>
    /// Mapeia a rota e o handler no <paramref name="app"/> fornecido.
    /// </summary>
    /// <param name="app">O <see cref="IEndpointRouteBuilder"/> onde a rota será registrada.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/transactions",
                async (
                    IMediator mediator,
                    [FromQuery] int pageNumber = 1,
                    [FromQuery] int pageSize = 10,
                    [FromQuery] Guid? userId = null,
                    [FromQuery] Guid? categoryId = null
                ) =>
                {
                    var query = new ListTransactionsQuery(pageNumber, pageSize, userId, categoryId);
                    var result = await mediator.Send(query);
                    return Results.Ok(result);
                }
            )
            .WithName("ListTransactions")
            .WithTags("Transactions");
    }
}
