using Domus.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Domus.Api.Features.Reports.TotalsByCategory;

public class GetTotalsByCategoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/reports/totals-by-category",
                async (IMediator mediator) =>
                {
                    var result = await mediator.Send(new GetTotalsByCategoryQuery());
                    return Results.Ok(result);
                }
            )
            .WithName("GetTotalsByCategory")
            .WithTags("Reports")
            .WithSummary("List all categories with their income, expense and balance totals.")
            .Produces<GetTotalsByCategoryResult>(StatusCodes.Status200OK);
    }
}
