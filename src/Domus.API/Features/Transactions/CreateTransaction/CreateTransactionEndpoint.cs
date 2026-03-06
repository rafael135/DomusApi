using Domus.Api.Extensions;
using Domus.Api.Features.Transactions.Shared;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Domus.Api.Features.Transactions.CreateTransaction;

/// <summary>
/// Endpoint para registro de uma nova transação financeira. Rota: <c>POST /api/transactions</c>.
/// </summary>
public class CreateTransactionEndpoint : IEndpoint
{
    /// <summary>
    /// Mapeia a rota e o handler no <paramref name="app"/> fornecido.
    /// </summary>
    /// <param name="app">O <see cref="IEndpointRouteBuilder"/> onde a rota será registrada.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/transactions",
                async (IMediator mediator, [FromBody] CreateTransactionCommand command) =>
                {
                    var result = await mediator.Send(command);
                    return Results.Ok(result.Transaction);
                }
            )
            .WithName("CreateTransaction")
            .WithTags("Transactions")
            .WithSummary("Create a new transaction.")
            .Produces<TransactionDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status422UnprocessableEntity);
    }
}
