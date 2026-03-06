using Domus.Api.Extensions;
using Domus.Api.Features.Categories.Shared;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Domus.Api.Features.Categories.CreateCategory;

public class CreateCategoryEndpoint : IEndpoint
{
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
