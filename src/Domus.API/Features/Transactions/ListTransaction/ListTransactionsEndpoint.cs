using Domus.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Domus.Api.Features.Transactions.ListTransaction;

public class ListTransactionsEndpoint : IEndpoint
{
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
