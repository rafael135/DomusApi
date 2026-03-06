using Domus.Api.Extensions;
using Domus.Api.Features.Transactions.Shared;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Domus.Api.Features.Transactions.CreateTransaction;

public class CreateTransactionEndpoint : IEndpoint
{
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
