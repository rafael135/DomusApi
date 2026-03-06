using Domus.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Domus.Api.Features.Reports.TotalsByPerson;

/// <summary>
/// Endpoint para o relatório de totais financeiros por pessoa. Rota: <c>GET /api/reports/totals-by-person</c>.
/// </summary>
public class GetTotalsByPersonEndpoint : IEndpoint
{
    /// <summary>
    /// Mapeia a rota e o handler no <paramref name="app"/> fornecido.
    /// </summary>
    /// <param name="app">O <see cref="IEndpointRouteBuilder"/> onde a rota será registrada.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/reports/totals-by-person",
                async (IMediator mediator) =>
                {
                    var result = await mediator.Send(new GetTotalsByPersonQuery());
                    return Results.Ok(result);
                }
            )
            .WithName("GetTotalsByPerson")
            .WithTags("Reports")
            .WithSummary("List all people with their income, expense and balance totals.")
            .Produces<GetTotalsByPersonResult>(StatusCodes.Status200OK);
    }
}
