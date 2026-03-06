using Domus.Api.Extensions;
using Domus.Core.Domain.Transactions.Enums;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Domus.Api.Features.Categories.ListCategories;

public class ListCategoriesEndpoint : IEndpoint
{
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
